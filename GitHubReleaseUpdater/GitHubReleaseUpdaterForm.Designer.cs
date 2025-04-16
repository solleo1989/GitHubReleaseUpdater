namespace GitHubReleaseUpdater
{
    partial class GitHubReleaseUpdaterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GitHubReleaseUpdaterForm));
            this.AppDataGridView = new System.Windows.Forms.DataGridView();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.DownloadButton = new System.Windows.Forms.Button();
            this.ReleasesTextBox = new System.Windows.Forms.RichTextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.DownloadAllButton = new System.Windows.Forms.Button();
            this.DownloadProgressBar = new System.Windows.Forms.ProgressBar();
            this.CopyFileLabel = new System.Windows.Forms.Label();
            this.DownloadListComboBox = new System.Windows.Forms.ComboBox();
            this.DownloadListLabel = new System.Windows.Forms.Label();
            this.ReleaseComboBox = new System.Windows.Forms.ComboBox();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.UserTokenLabel = new System.Windows.Forms.Label();
            this.UserTokenTextBox = new System.Windows.Forms.TextBox();
            this.UserInfoUpdateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AppDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // AppDataGridView
            // 
            this.AppDataGridView.AllowUserToResizeColumns = false;
            this.AppDataGridView.AllowUserToResizeRows = false;
            this.AppDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AppDataGridView.Location = new System.Drawing.Point(49, 89);
            this.AppDataGridView.MultiSelect = false;
            this.AppDataGridView.Name = "AppDataGridView";
            this.AppDataGridView.RowHeadersWidth = 72;
            this.AppDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.AppDataGridView.RowTemplate.Height = 31;
            this.AppDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AppDataGridView.Size = new System.Drawing.Size(1784, 800);
            this.AppDataGridView.TabIndex = 0;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(50, 903);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(150, 70);
            this.RefreshButton.TabIndex = 1;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(450, 903);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(150, 70);
            this.LoadButton.TabIndex = 2;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // DownloadButton
            // 
            this.DownloadButton.Enabled = false;
            this.DownloadButton.Location = new System.Drawing.Point(1000, 903);
            this.DownloadButton.Name = "DownloadButton";
            this.DownloadButton.Size = new System.Drawing.Size(150, 70);
            this.DownloadButton.TabIndex = 3;
            this.DownloadButton.Text = "Download";
            this.DownloadButton.UseVisualStyleBackColor = true;
            this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // ReleasesTextBox
            // 
            this.ReleasesTextBox.Font = new System.Drawing.Font("Microsoft YaHei", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReleasesTextBox.Location = new System.Drawing.Point(1839, 89);
            this.ReleasesTextBox.Name = "ReleasesTextBox";
            this.ReleasesTextBox.ReadOnly = true;
            this.ReleasesTextBox.Size = new System.Drawing.Size(750, 800);
            this.ReleasesTextBox.TabIndex = 4;
            this.ReleasesTextBox.Text = "";
            this.ReleasesTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ReleasesTextBox_LinkClicked);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(250, 903);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(150, 70);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DownloadAllButton
            // 
            this.DownloadAllButton.Enabled = false;
            this.DownloadAllButton.Location = new System.Drawing.Point(1200, 903);
            this.DownloadAllButton.Name = "DownloadAllButton";
            this.DownloadAllButton.Size = new System.Drawing.Size(150, 70);
            this.DownloadAllButton.TabIndex = 6;
            this.DownloadAllButton.Text = "Download All";
            this.DownloadAllButton.UseVisualStyleBackColor = true;
            this.DownloadAllButton.Click += new System.EventHandler(this.DownloadAllButton_Click);
            // 
            // DownloadProgressBar
            // 
            this.DownloadProgressBar.Location = new System.Drawing.Point(1400, 910);
            this.DownloadProgressBar.Name = "DownloadProgressBar";
            this.DownloadProgressBar.Size = new System.Drawing.Size(224, 50);
            this.DownloadProgressBar.TabIndex = 7;
            // 
            // CopyFileLabel
            // 
            this.CopyFileLabel.AutoSize = true;
            this.CopyFileLabel.Location = new System.Drawing.Point(1496, 864);
            this.CopyFileLabel.Name = "CopyFileLabel";
            this.CopyFileLabel.Size = new System.Drawing.Size(0, 25);
            this.CopyFileLabel.TabIndex = 8;
            this.CopyFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DownloadListComboBox
            // 
            this.DownloadListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DownloadListComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadListComboBox.FormattingEnabled = true;
            this.DownloadListComboBox.Location = new System.Drawing.Point(266, 30);
            this.DownloadListComboBox.Name = "DownloadListComboBox";
            this.DownloadListComboBox.Size = new System.Drawing.Size(571, 37);
            this.DownloadListComboBox.TabIndex = 9;
            this.DownloadListComboBox.SelectedIndexChanged += new System.EventHandler(this.DownloadListComboBox_SelectedIndexChanged);
            // 
            // DownloadListLabel
            // 
            this.DownloadListLabel.AutoSize = true;
            this.DownloadListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadListLabel.Location = new System.Drawing.Point(44, 30);
            this.DownloadListLabel.Name = "DownloadListLabel";
            this.DownloadListLabel.Size = new System.Drawing.Size(201, 32);
            this.DownloadListLabel.TabIndex = 10;
            this.DownloadListLabel.Text = "Download List:";
            // 
            // ReleaseComboBox
            // 
            this.ReleaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReleaseComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReleaseComboBox.FormattingEnabled = true;
            this.ReleaseComboBox.ItemHeight = 29;
            this.ReleaseComboBox.Location = new System.Drawing.Point(627, 910);
            this.ReleaseComboBox.Name = "ReleaseComboBox";
            this.ReleaseComboBox.Size = new System.Drawing.Size(348, 37);
            this.ReleaseComboBox.TabIndex = 11;
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.UserNameLabel.Location = new System.Drawing.Point(900, 35);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(122, 25);
            this.UserNameLabel.TabIndex = 12;
            this.UserNameLabel.Text = "User name:";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(1050, 35);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(200, 29);
            this.UserNameTextBox.TabIndex = 13;
            // 
            // UserTokenLabel
            // 
            this.UserTokenLabel.AutoSize = true;
            this.UserTokenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.UserTokenLabel.Location = new System.Drawing.Point(1270, 35);
            this.UserTokenLabel.Name = "UserTokenLabel";
            this.UserTokenLabel.Size = new System.Drawing.Size(122, 25);
            this.UserTokenLabel.TabIndex = 14;
            this.UserTokenLabel.Text = "User token:";
            // 
            // UserTokenTextBox
            // 
            this.UserTokenTextBox.Location = new System.Drawing.Point(1400, 35);
            this.UserTokenTextBox.Name = "UserTokenTextBox";
            this.UserTokenTextBox.Size = new System.Drawing.Size(200, 29);
            this.UserTokenTextBox.TabIndex = 15;
            // 
            // UserInfoUpdateButton
            // 
            this.UserInfoUpdateButton.Location = new System.Drawing.Point(1650, 25);
            this.UserInfoUpdateButton.Name = "UserInfoUpdateButton";
            this.UserInfoUpdateButton.Size = new System.Drawing.Size(170, 50);
            this.UserInfoUpdateButton.TabIndex = 16;
            this.UserInfoUpdateButton.Text = "Update";
            this.UserInfoUpdateButton.UseVisualStyleBackColor = true;
            this.UserInfoUpdateButton.Click += new System.EventHandler(this.UserInfoUpdateButton_Click);
            // 
            // GitHubReleaseUpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2917, 1000);
            this.Controls.Add(this.UserInfoUpdateButton);
            this.Controls.Add(this.UserTokenTextBox);
            this.Controls.Add(this.UserTokenLabel);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.UserNameLabel);
            this.Controls.Add(this.ReleaseComboBox);
            this.Controls.Add(this.DownloadListLabel);
            this.Controls.Add(this.DownloadListComboBox);
            this.Controls.Add(this.CopyFileLabel);
            this.Controls.Add(this.DownloadProgressBar);
            this.Controls.Add(this.DownloadAllButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ReleasesTextBox);
            this.Controls.Add(this.DownloadButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.AppDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GitHubReleaseUpdaterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GitHub Release Updater";
            ((System.ComponentModel.ISupportInitialize)(this.AppDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView AppDataGridView;

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button DownloadButton;
        private System.Windows.Forms.Button DownloadAllButton;
        private System.Windows.Forms.RichTextBox ReleasesTextBox;
        private System.Windows.Forms.ProgressBar DownloadProgressBar;
        private System.Windows.Forms.Label CopyFileLabel;
        private System.Windows.Forms.ComboBox DownloadListComboBox;
        private System.Windows.Forms.Label DownloadListLabel;
        private System.Windows.Forms.ComboBox ReleaseComboBox;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label UserTokenLabel;
        private System.Windows.Forms.TextBox UserTokenTextBox;
        private System.Windows.Forms.Button UserInfoUpdateButton;
    }
}

