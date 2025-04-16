using GitHubReleaseUpdater.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace GitHubReleaseUpdater.Helpers
{
    public class ActionHelper
    {
        private string FileDirectory = Environment.CurrentDirectory;
        private string RepoFilePrefix = "";
        private string RepoFileSuffix = ".txt";

        private string TimeFilePrefix = "timeline";

        public string GetPath(string path) => !path.EndsWith("\\") ? path + "\\" : path;
        public string ReplaceInvalidChars(string filename) => string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        public string BracketFolder(string folder) => ReplaceInvalidChars(folder.Replace("[", "``[").Replace("]", "``]"));

        public string GetCurrentRepoPath(string name) => GetPath(FileDirectory) + "repo\\" + RepoFilePrefix + name + RepoFileSuffix;
        public string GetCurrentTimePath() => GetPath(FileDirectory) + TimeFilePrefix;
        public string GetUserTokenPath() => GetPath(FileDirectory) + "usertoken";

        public string CreateFolder(string path, string folder)
        {
            var foldername = GetPath(path) + ReplaceInvalidChars(folder);
            if (Directory.Exists(path))
            {
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                return foldername;
            }
            return null;
        }

        /// <summary>
        /// Fetch the repository information saved under "repo" folder
        /// </summary>
        /// <returns></returns>
        public string[] LoadRepoFileNames()
        {
            try
            {
                if (!Directory.Exists(GetPath(FileDirectory) + "repo\\"))
                {
                    Directory.CreateDirectory(GetPath(FileDirectory) + "repo\\");
                }
            }
            catch (Exception ex)
            {
            }
            IEnumerable<string> txtFiles = Directory.EnumerateFiles(GetPath(FileDirectory) + "repo\\", "*.txt");
            var list = new List<string>();
            foreach (string currentFile in txtFiles)
            {
                var filename = new FileInfo(currentFile).Name;
                filename = filename.Replace(this.RepoFileSuffix, "");
                list.Add(filename);
            }   
            return list.ToArray();
        }

        public string[] LoadPowerShellFile(string filepath)
        {
            var file = new FileInfo(filepath);
            if (!file.Exists)
            {
                return null;
            }
            return File.ReadAllLines(file.FullName);
        }
        #region Repository

        /// <summary>
        /// Load repository file info for the app
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeMapping"></param>
        /// <returns></returns>
        public List<RepoModel> LoadRepositoryFileInfo(string name, Dictionary<string, string> timeMapping)
        {
            var filePath = this.GetCurrentRepoPath(name);
            var file = new FileInfo(filePath);
            var repos = new List<RepoModel>();
            if (!file.Exists)
            {
                file.Create();
            }
            else
            {
                string[] lines = File.ReadAllLines(file.FullName);
                var repo = new RepoModel();
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {   // [repo | latest release]
                        string patternT = @"^[\[]([-a-zA-Z0-9._@#$%]+)\s|\s([-a-zA-Z0-9._@#$%]+)[\]]$";
                        Match m1 = Regex.Match(line, patternT, RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            var titles = line.Substring(1, line.Length - 2).Split('|');
                            repo = new RepoModel(titles[0].Trim(), titles[1].Trim());
                        }
                        else if (line.StartsWith("|CATE"))
                        {
                            var title = line.Substring(6);
                            repo.Category = title.Trim();
                        }
                        else if (line.StartsWith("|PATH"))
                        {
                            var title = line.Substring(6);
                            repo.LocalPath = title.Trim();
                        }
                        else if (line.StartsWith("|CR"))
                        {
                            var title = line.Substring(4);
                            repo.CurrentRelease = new ReleaseModel(title.Trim());
                        }
                        else if (line.StartsWith("|END"))
                        {
                            var key = repo.User + '\t' + repo.Repo;
                            if (timeMapping.ContainsKey(key))
                            {
                                var time = timeMapping[key];
                                repo.LastUpdateDateTime = DateTime.Parse(time.Trim());
                            }
                            repos.Add(repo);
                        }
                    }
                }
            }

            return repos;
        }

        public void SaveRepositoryDataFile(string name, List<RepoModel> repos)
        {
            var filePath = this.GetCurrentRepoPath(name);
            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Create), Encoding.ASCII))
            {
                foreach (var repo in repos)
                {
                    string content = $"[{repo.User} | {repo.Repo}]\r\n"
                        + $"|CATE {repo.Category}\r\n"
                        + $"|PATH {repo.LocalPath}\r\n"
                        + $"|CR {repo.CurrentRelease?.Name}\r\n"
                        + $"|TIME {repo.GetLastUpdateDateTime()}\r\n"
                        + $"|END\r\n";
                    sw.WriteLine(content);
                }
            }
        }
        #endregion

        #region Timeline
        public void SaveTimeLineFile(string user, string repo, string lastUpdateTime)
        {
            var filePath = this.GetCurrentTimePath();
            string content = $"{user}\t{repo}\t{lastUpdateTime}\r\n";
            File.AppendAllText(filePath, string.Format("{0}{1}", content, Environment.NewLine));
        }

        /// <summary>
        /// Load the latest saved date time mapping for each user-repo
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetDateTimeMapping()
        {
            var filePath = this.GetCurrentTimePath();
            var file = new FileInfo(filePath);

            var mapping = new Dictionary<string, string>();
            if (file.Exists)
            {
                string[] lines = File.ReadAllLines(file.FullName);                
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line)) {
                        string[] parts = line.Split('\t');
                        var key = parts[0] + '\t' + parts[1];
                        if (!mapping.ContainsKey(key))
                        {
                            mapping.Add(key, parts[2]);
                        }
                        else
                        { 
                            mapping[key] = parts[2];
                        }
                    }
                }
            }
            return mapping;
        }
        #endregion

        public void SaveContentsToFile(string[] contents, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Create), Encoding.ASCII))
            {
                foreach (var content in contents)
                {
                    sw.WriteLine(content);
                }
            }
        }

        public void UpdateDataFile(string name, List<RepoModel> repos)
        {
            var filePath = this.GetCurrentRepoPath(name);
            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Create), Encoding.ASCII))
            {
                foreach (var repo in repos)
                {
                    string content = $"[{repo.User} | {repo.Repo}]\r\n"
                        + $"|CATE {repo.Category}\r\n"
                        + $"|PATH {repo.LocalPath}\r\n"
                        + $"|CR {repo.LatestRelease.Name}\r\n"
                        + $"|TIME {repo.GetLastUpdateDateTime()}\r\n"
                        + $"|END\r\n";
                    sw.WriteLine(content);
                }
            }
        }

        public void OpenGameDirectory(string path)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (path.StartsWith("http"))
                {
                    Process p = new Process();
                    p = Process.Start("IExplore.exe", path);
                }
                else if (path.StartsWith("file"))
                {

                    var filePath = new Uri(path).AbsolutePath;
                    path = path.Replace("/", "\\");
                    Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", path);
                }
            }
        }

        public IEnumerable<string> LoadUserTokenInfo()
        {
            var filePath = this.GetUserTokenPath();
            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                return File.ReadAllLines(file.FullName).ToArray<string>().Where(l => !string.IsNullOrEmpty(l));
            }
            return new List<string>();
        }

        public void UpdateUserTokenInfo(string userName, string userToken)
        {
            var filePath = this.GetUserTokenPath();
            using (StreamWriter sw = new StreamWriter(File.Open(filePath, System.IO.FileMode.Create), Encoding.ASCII))
            {
                string content = $"{userName}\r\n"
                        + $"{userToken}\r\n";
                sw.WriteLine(content);
            }
        }
    }
}
