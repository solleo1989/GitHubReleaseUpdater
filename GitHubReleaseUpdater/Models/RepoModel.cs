using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseUpdater.Models
{
    public class RepoModel
    {
        public string User;
        public string Repo;
        public string Category;
        public string LocalPath;
        public IEnumerable<ReleaseModel> Releases;
        public ReleaseModel LatestRelease;
        public ReleaseModel CurrentRelease;
        public DateTime LastUpdateDateTime;

        private const string ContentSplitter = " - ";

        public RepoModel() { }

        public RepoModel(string user, string repo) 
        { 
            this.User = user;
            this.Repo = repo;
        }

        public String GetLastUpdateDateTime()
        { 
            if (this.LastUpdateDateTime == default(DateTime))
            {
                return "";
            }
            return this.LastUpdateDateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        /// <summary>
        /// Override the toString function
        /// </summary>
        /// <returns>return the formatted output string.</returns>
        public override string ToString()
        {
            DirectoryInfo folder = new DirectoryInfo(this.LocalPath);
            List<string> collection = folder.Exists 
                ? folder.GetDirectories().Select(f => f.Name).ToList() 
                : new List<string>();

            string result = $"[{this.User} | {this.Repo}]\r\n"
                + $"{ContentSplitter}Category: {this.Category}\r\n"
                + $"{ContentSplitter}Url: https://github.com/{this.User}/{this.Repo}\r\n"
                + $"{ContentSplitter}LocalPath: file://{this.LocalPath.Replace("\\", "/") + "/"}\r\n"
                + $"{ContentSplitter}CurrentRelease: {this.CurrentRelease?.Name}\r\n"
                + $"{ContentSplitter}LatestRelease: {this.LatestRelease?.ToLongString()}\r\n"
                + $"{ContentSplitter}LastUpdateDateTime: {this.GetLastUpdateDateTime()}\r\n"
                + $">>>> Releases <<<<\r\n"
                + ((this.Releases != null && this.Releases.ToList().Count > 0) 
                    ? "" + string.Join("\r\n", 
                        this.Releases.Select(r => GetDownloadString(r, collection)).ToArray<string>()) 
                    : "\r\n");
            return result;
        }

        private string GetDownloadString(ReleaseModel r, List<string> folderContained)
        {
            var rname = $"[{r.Index}] {r.ToShortString()}";

            // Once the folder was downloaded, add a tickmark.
            return folderContained.Contains(rname) ? rname + " \u221A" : rname;
        }
    }
}
