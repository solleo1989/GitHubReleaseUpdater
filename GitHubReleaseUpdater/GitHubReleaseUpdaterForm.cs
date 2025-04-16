using GitHubReleaseUpdater.Builders;
using GitHubReleaseUpdater.Helpers;
using GitHubReleaseUpdater.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace GitHubReleaseUpdater
{
    public partial class GitHubReleaseUpdaterForm : Form
    {
        private LoadingForm LoadingForm;
        private ActionHelper Action;
        private List<RepoModel> List;
        private Dictionary<string, string> LastSavedTimeMapping;
        private UserModel User;

        public GitHubReleaseUpdaterForm(ActionHelper actionHelper)
        {
            this.Action = actionHelper;
            this.LoadingForm = new LoadingForm();

            InitializeComponent();
            InitializeForm();
            RefreshForm();
            UpdateTextboxContents();
        }

        public void InitializeForm()
        {
            var usertoken = this.Action.LoadUserTokenInfo().ToArray();
            this.User = usertoken != null && usertoken.Length == 2
                ? new UserModel(usertoken[0], usertoken[1])
                : new UserModel();
            var filenames = this.Action.LoadRepoFileNames();
            this.LastSavedTimeMapping = this.Action.GetDateTimeMapping();
            this.DownloadListComboBox.Items.Clear();
            if (filenames != null && filenames.Length > 0)
            {
                this.DownloadListComboBox.Items.AddRange(filenames);
                this.DownloadListComboBox.SelectedIndex = 0;
            }
            this.ReleaseComboBox.Enabled = false;
        }

        public void RefreshForm()
        {
            BuildDataGridView();
            DisableDownloadButtons();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.AppDataGridView.SelectionChanged += new EventHandler(AppDataGridView_SelectionChanged);

            base.OnLoad(e);
        }

        public void BuildDataGridView() {
            this.AppDataGridView.Rows.Clear();
            this.AppDataGridView.BackgroundColor = Color.White;
            this.AppDataGridView.RowHeadersVisible = false;
            this.AppDataGridView.ColumnCount = 7;

            this.AppDataGridView.Columns[0].Name = "User";
            this.AppDataGridView.Columns[0].Width = 100;

            this.AppDataGridView.Columns[1].Name = "Repository";
            this.AppDataGridView.Columns[1].Width = 120;

            this.AppDataGridView.Columns[2].Name = "Category";
            this.AppDataGridView.Columns[2].Width = 100;

            this.AppDataGridView.Columns[3].Name = "LocalPath";
            this.AppDataGridView.Columns[3].Width = 260;

            this.AppDataGridView.Columns[4].Name = "Current Release";
            this.AppDataGridView.Columns[4].Width = 120;
            this.AppDataGridView.Columns[4].ReadOnly = true;

            this.AppDataGridView.Columns[5].Name = "Latest Release";
            this.AppDataGridView.Columns[5].Width = 120;
            this.AppDataGridView.Columns[5].ReadOnly = true;

            this.AppDataGridView.Columns[6].Name = "Last Update Time";
            this.AppDataGridView.Columns[6].Width = 120;
            this.AppDataGridView.Columns[6].ReadOnly = true;
            if (this.List == null || this.List.Count == 0)
            {
                //this.AppDataGridView.Rows.Add("", "", "");
            }
            else
            {
                foreach (var repo in this.List)
                {
                    this.AppDataGridView.Rows.Add(
                        repo.User,
                        repo.Repo,
                        repo.Category,
                        repo.LocalPath,
                        repo.CurrentRelease?.Name,
                        "",
                        repo.GetLastUpdateDateTime()
                        );
                }
            }
            this.AppDataGridView.Rows[0].Selected = true;

            this.AppDataGridView.Dock = DockStyle.None;
            this.AppDataGridView.Sorted += new EventHandler(this.DataGridView_Sorted);
            // Update user toekn info
            this.UserInfoUpdateButton.Text = this.User.IsDummyUser
                ? "Create"
                : "Update";
            this.UserNameTextBox.Text = this.User.UserName;
            this.UserTokenTextBox.Text = this.User.UserToken;
            ValidateUserToken();
        }

        public void SaveDataInformationIntoLocalFile()
        {
            this.AppDataGridView.CurrentCell = this.AppDataGridView.SelectedRows[0].Cells[0];
            List<RepoModel> repos = new List<RepoModel>();
            foreach (DataGridViewRow row in this.AppDataGridView.Rows)
            {
                if (!string.IsNullOrWhiteSpace((string)row.Cells[0].Value)
                    && !string.IsNullOrWhiteSpace((string)row.Cells[1].Value))
                {
                    var model = new RepoModel()
                    {
                        User = row.Cells[0].Value.ToString(),
                        Repo = row.Cells[1].Value.ToString(),
                        Category = ((row.Cells[2])?.Value)?.ToString(),
                        LocalPath = ((row.Cells[3])?.Value)?.ToString(),
                        CurrentRelease = new ReleaseModel(((row.Cells[4])?.Value)?.ToString()),
                        LatestRelease = new ReleaseModel(((row.Cells[5])?.Value)?.ToString()),
                    };
                    repos.Add(model);
                }
            }
            var name = this.DownloadListComboBox.SelectedItem.ToString();
            this.Action.SaveRepositoryDataFile(name, repos);
            PopupInfoMessage("Save", "Your data has been saved successfully!");
            this.List = this.Action.LoadRepositoryFileInfo(name, this.LastSavedTimeMapping);
            this.RefreshForm();
        }

        public void DownloadAndUpdateDataInfo(int index)
        {
            if (this.AppDataGridView.SelectedRows.Count == 1)
            {
                var currentRow = this.AppDataGridView.SelectedRows[0];
                this.AppDataGridView.CurrentCell = currentRow.Cells[0];
                if (!string.IsNullOrWhiteSpace((string)currentRow.Cells[0].Value)
                    && !string.IsNullOrWhiteSpace((string)currentRow.Cells[1].Value))
                {
                    var model = this.List[currentRow.Index];

                    if (model.Releases != null && model.Releases.Count() > 0)
                    {
                        // Create folder and Download
                        var release = (model.Releases.ToList())[model.Releases.Count() - index];
                        var date = release.CreatedTime.ToString("yyyyMMdd");
                        var foldername = $"[{release.Index}] {release.Name} ({date})";
                        var folderpath = this.Action.CreateFolder(model.LocalPath, foldername);
                        var zipfolderpath = this.Action.CreateFolder(folderpath, "Source code (zip)");

                        DownLoadZipballFromModel(zipfolderpath, release);
                        DownLoadAssetsFromModel(folderpath, release.Assets);

                        // Update time file
                        model.LastUpdateDateTime = DateTime.Now;
                        Action.SaveTimeLineFile(model.User, model.Repo, model.LastUpdateDateTime.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    UpdateDataGridViewAfterDownloadCompete(model, currentRow);
                }
            }
            //RefreshForm();
            PopupInfoMessage("Download", "Your data has been downloaded and updated successfully!");
            // Refresh selected row
            var rIndex = this.AppDataGridView.SelectedRows[0].Index;
            this.AppDataGridView.ClearSelection();
            this.AppDataGridView.Rows[rIndex].Selected = true;
        }

        public void DownloadAllData()
        {
            if (this.AppDataGridView.SelectedRows.Count == 1)
            {
                var currentRow = this.AppDataGridView.SelectedRows[0];
                this.AppDataGridView.CurrentCell = currentRow.Cells[0];
                if (!string.IsNullOrWhiteSpace((string)currentRow.Cells[0].Value)
                    && !string.IsNullOrWhiteSpace((string)currentRow.Cells[1].Value))
                {
                    var model = this.List[currentRow.Index];
                    if (model.Releases != null && model.Releases.Count() > 0)
                    {
                        // Create folder and Download for each release
                        for (var index = 0; index < model.Releases.Count(); index++)
                        {
                            var release = (model.Releases.ToList())[index];
                            var date = release.CreatedTime.ToString("yyyyMMdd");
                            var foldername = $"[{release.Index}] {release.Name} ({date})";
                            var folderpath = this.Action.CreateFolder(model.LocalPath, foldername);
                            var zipfolderpath = this.Action.CreateFolder(folderpath, "Source code (zip)");

                            DownLoadZipballFromModel(zipfolderpath, release);
                            DownLoadAssetsFromModel(folderpath, release.Assets);
                        }
                    }

                    UpdateDataGridViewAfterDownloadCompete(model, currentRow);
                }
            }
            //RefreshForm();
            PopupInfoMessage("Download", "Your data has been downloaded and updated successfully!");
        }

        private void DownLoadZipballFromModel(string zipfolderpath, ReleaseModel release)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.Headers.Add("user-agent", "Anything");
                var zipFilePath = this.Action.GetPath(zipfolderpath)
                    + this.Action.ReplaceInvalidChars(release.Name) + ".zip";
                wc.DownloadFile(new Uri(release.ZipFileUrl), zipFilePath);
                // wait for the current thread to complete, since the an async action will be on a new thread.
                while (wc.IsBusy) { }
            }
        }

        /// <summary>
        /// Download all assets from the model, open the folder if downloaded files include zip file.
        /// </summary>
        /// <param name="folderpath"></param>
        /// <param name="assets"></param>
        private void DownLoadAssetsFromModel(string folderpath, List<AssetModel> assets)
        {
            bool hasZipFile = false;
            var zipFiles = new List<String>();
            foreach (var asset in assets)
            {
                using (WebClient wc = new WebClient())
                {
                    this.CopyFileLabel.Text = asset.Name;
                    wc.Headers.Add("user-agent", "Anything");
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    var assetPath = this.Action.GetPath(folderpath) + asset.Name;
                    wc.DownloadFileAsync(
                        new Uri(asset.DownloadUrl),     // Param1 = Link of file
                        assetPath                       // Param2 = Path to save
                    );
                    if (asset.Name.EndsWith("zip") || asset.Name.EndsWith("rar") || asset.Name.EndsWith("7z"))
                    {
                        hasZipFile = true;
                        zipFiles.Add(assetPath);
                    }
                    // wait for the current thread to complete, since the an async action will be on a new thread.
                    while (wc.IsBusy) { }
                }
            }
            if (hasZipFile)
            {
                // TODO: fix the logic of zip/unzip source items
                foreach (var assetPath in zipFiles)
                {
                    //ZipArchive archive = ZipFile.Open(assetPath, ZipArchiveMode.Update);

                    //var assetPathWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
                    //ZipFile.ExtractToDirectory(assetPath, this.Action.GetPath(folderpath) + assetPathWithoutExtension);
                    //archive.clo
                }

                Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", folderpath);
            }
        }

        private void UpdateDataGridViewAfterDownloadCompete(RepoModel model, DataGridViewRow currentRow)
        {
            model.CurrentRelease = model.LatestRelease;
            //model.LastUpdateDateTime = DateTime.Now;
            this.List[currentRow.Index] = model;
            this.CopyFileLabel.Text = "Completed!";
            this.AppDataGridView.Rows[currentRow.Index].Cells[4].Value = this.AppDataGridView.Rows[currentRow.Index].Cells[5].Value;
            this.AppDataGridView.Rows[currentRow.Index].Cells[4].Style.BackColor = Color.Green;
            this.AppDataGridView.Rows[currentRow.Index].Cells[6].Value = model.GetLastUpdateDateTime();
            var name = this.DownloadListComboBox.SelectedItem.ToString();
            this.Action.SaveRepositoryDataFile(name, this.List);
        }

        public void UpdateAllInfo()
        {
            this.AppDataGridView.CurrentCell = this.AppDataGridView.SelectedRows[0].Cells[0];
            List<RepoModel> repos = new List<RepoModel>();
            foreach (DataGridViewRow row in this.AppDataGridView.Rows)
            {
                if (!string.IsNullOrWhiteSpace((string)row.Cells[0].Value)
                    && !string.IsNullOrWhiteSpace((string)row.Cells[1].Value))
                {
                    var model = new RepoModel()
                    {
                        User = row.Cells[0].Value.ToString(),
                        Repo = row.Cells[1].Value.ToString(),
                        Category = ((row.Cells[2])?.Value)?.ToString(),
                        LocalPath = ((row.Cells[3])?.Value)?.ToString(),
                        LatestRelease = new ReleaseModel(((row.Cells[5])?.Value)?.ToString()),
                    };
                    repos.Add(model);
                }
            }
            var name = this.DownloadListComboBox.SelectedItem.ToString();
            this.Action.SaveRepositoryDataFile(name, repos);
        }

        public void LoadFromGitHub()
        {
            var index = this.AppDataGridView.SelectedRows[0].Index;
            index = index >= this.List.Count ? 0 : index;
            List<RepoModel> repos = new List<RepoModel>();
            ShowLoadingForm();

            foreach (var repo in this.List)
            {
                if (!string.IsNullOrWhiteSpace(repo.User) && !string.IsNullOrWhiteSpace(repo.Repo))
                {
                    RepoModelBuilder builder = new RepoModelBuilder(this.User);
                    var model = builder.BuildRepoModel(repo.User, repo.Repo);
                    model.Category = repo.Category;
                    model.LocalPath = repo.LocalPath;
                    model.CurrentRelease = repo.CurrentRelease;
                    model.LastUpdateDateTime = repo.LastUpdateDateTime;
                    repos.Add(model);
                }
            }

            this.List = repos;
            this.AppDataGridView.Rows.Clear();
            var counter = 0;
            foreach (var repo in repos)
            {
                this.AppDataGridView.Rows.Add(
                    repo.User,
                    repo.Repo,
                    repo.Category,
                    repo.LocalPath,
                    repo.CurrentRelease?.Name,
                    repo.LatestRelease?.Name,
                    repo.GetLastUpdateDateTime());
                if (repo.CurrentRelease?.Name == repo.LatestRelease?.Name)
                {
                    this.AppDataGridView.Rows[counter].Cells[4].Style.BackColor = Color.Green;
                }
                else
                {
                    this.AppDataGridView.Rows[counter].Cells[4].Style.BackColor = Color.Red;
                }
                counter++;
            }

            this.AppDataGridView.Rows[index].Selected = true;
            this.EnableDownloadButtons();
            this.HideLoadingForm();
        }

        public void BuildReleaseDropdownList(IEnumerable<ReleaseModel> releases)
        {
            this.ReleaseComboBox.Items.Clear();
            int count = releases.Count() > 0 ? releases.Count() : 0;
            foreach (var release in releases)
            {
                this.ReleaseComboBox.Items.Add($"[{count--}] {release.ToShortString()}");
            }
            this.ReleaseComboBox.SelectedIndex = 0;
        }

        public void UpdateTextboxContents()
        {
            if (this.AppDataGridView != null && this.AppDataGridView.SelectedRows.Count > 0)
            {
                var currentRow = this.AppDataGridView.SelectedRows[0];
                if (currentRow != null && this.List != null && currentRow.Index < this.List.Count) {
                    var model = this.List[currentRow.Index];

                    this.ReleasesTextBox.Text = model.ToString();
                    // Disable buttons and dropdownlist for non-release build.
                    this.DownloadButton.Enabled = model.LatestRelease != null;
                    this.ReleaseComboBox.Enabled = model.LatestRelease != null;
                    if (model.LatestRelease != null)
                    {
                        BuildReleaseDropdownList(model.Releases);
                    }
                }
            }
        }

        #region Form action

        private void UserInfoUpdateButton_Click(object sender, EventArgs e)
        {
            var username = this.UserNameTextBox.Text;
            var usertoken = this.UserTokenTextBox.Text;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(usertoken))
            {
                this.Action.UpdateUserTokenInfo(username, usertoken);
                PopupInfoMessage("Update", "Your user token information has been updated successfully!");
                this.UserInfoUpdateButton.Text = "Update";
                this.User.UserName = username;
                this.User.UserToken = usertoken;
                ValidateUserToken();
            }
        }

        private void AppDataGridView_SelectionChanged(object sender, EventArgs e) => UpdateTextboxContents();

        private void RefreshButton_Click(object sender, EventArgs e) => RefreshForm();

        private void SaveButton_Click(object sender, EventArgs e) => SaveDataInformationIntoLocalFile();

        private void LoadButton_Click(object sender, EventArgs e) => LoadFromGitHub();

        private void DownloadListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = this.DownloadListComboBox.SelectedItem.ToString();
            this.List = this.Action.LoadRepositoryFileInfo(name, this.LastSavedTimeMapping);
            RefreshForm();
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            var currentIndex = this.ReleaseComboBox.Items.Count - this.ReleaseComboBox.SelectedIndex;
            DownloadAndUpdateDataInfo(currentIndex);
        }

        private void DownloadAllButton_Click(object sender, EventArgs e) => DownloadAllData();

        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            this.DownloadProgressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void ReleasesTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            this.Action.OpenGameDirectory(e.LinkText);
        }

        /// <summary>
        /// This function needs to be called after the sort applied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_Sorted(object sender, EventArgs e)
        {
            if (this.List != null && this.AppDataGridView != null)
            {
                var newList = new List<RepoModel>();
                for (int i = 0; i < this.AppDataGridView.RowCount; i++)
                {
                    var user = this.AppDataGridView.Rows[i].Cells[0].Value;
                    var repo = this.AppDataGridView.Rows[i].Cells[1].Value;
                    var item = this.List.Where(repomodel => repomodel.User.Equals(user) && repomodel.Repo.Equals(repo)).FirstOrDefault();
                    if (item != null)
                    {
                        newList.Add(item);
                    }
                }
                this.List = newList;
            }
        }

        #endregion

        public void PopupInfoMessage(string title, string message)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                //this.Close();
            }
        }

        /// <summary>
        /// Show loading form.
        /// </summary>
        private void ShowLoadingForm()
        {
            this.Hide();
            this.LoadingForm.Show();
        }

        /// <summary>
        /// Hide loading form.
        /// </summary>
        private void HideLoadingForm()
        {
            this.LoadingForm.Hide();
            this.Show();
        }

        private void EnableDownloadButtons()
        {
            this.DownloadButton.Enabled = true;
            this.DownloadAllButton.Enabled = true;
        }

        private void DisableDownloadButtons()
        {
            this.DownloadButton.Enabled = false;
            this.DownloadAllButton.Enabled = false;
        }

        private void ValidateUserToken()
        {
            this.LoadButton.Enabled = this.User.IsDummyUser ? false : true;
            this.DownloadButton.Enabled = this.User.IsDummyUser ? false : true;
            this.DownloadAllButton.Enabled = this.User.IsDummyUser ? false : true;
        }
    }
}
