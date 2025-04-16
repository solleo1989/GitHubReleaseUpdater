using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseUpdater.Models
{
    public enum ReleaseType { Stable, Beta, PreRelease }

    public class ReleaseModel
    {
        public int Index;
        public int Id;
        public string Name;
        public bool IsPreRelease;
        public string HtmlUrl;
        public DateTimeOffset CreatedTime;
        public string ZipFileUrl;
        //public string TagName;
        //public string Version;
        public string DescriptionShort;
        public string DescriptionLong;
        public List<AssetModel> Assets;

        public ReleaseType Type = ReleaseType.Stable;

        public string FriendlyUrl;
        public IList<string> DownloadUrl = new List<string>();

        public ReleaseModel() { 
        }
        public ReleaseModel(string name)
        {
            this.Name = name;
        }

        public string DownloadUrl32Bit
        {
            get
            {
                string url = DownloadUrl
                    .FirstOrDefault(x => !x.ToLower().Contains("64-bit") && !x.ToLower().Contains("64bit"));
                return url ?? "";
            }
        }
        public string DownloadUrl64Bit
        {
            get
            {
                string url = DownloadUrl
                    .FirstOrDefault(x => x.ToLower().Contains("64-bit") || x.ToLower().Contains("64bit"));
                return url ?? "";
            }
        }


        public IList<string> Features
        {
            get
            {
                string[] lines = DescriptionLong.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<string> feats = new List<string>();
                foreach (string line in lines)
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("- ") || trimmed.StartsWith("* "))
                        feats.Add(trimmed.Substring(2).Trim());
                }
                return feats;
            }
        }


        public string ToShortString()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return "";
            }
            else if (this.CreatedTime == null)
            {
                return this.Name;
            }
            return $"{this.Name} ({this.CreatedTime.ToString("yyyyMMdd")})";
        }

        public string ToLongString()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return "";
            }
            else if (this.CreatedTime == null)
            {
                return this.Name;
            }
            return $"{this.Name} ({this.CreatedTime.ToString("yyyyMMdd")})\r\n"
                    + $"[{this.DescriptionShort}]";
        }

        public override string ToString()
        {
            string friendly = DescriptionShort + " (" + Name + ")";
            var feats = Features;
            if (feats.Count > 0)
            {
                feats = feats.Where(f => !string.IsNullOrEmpty(f))?.Select(x => "* " + x).ToList();
                friendly += "\r\n" + string.Join("\r\n", feats);
            }
            friendly += !string.IsNullOrEmpty(DownloadUrl32Bit) ? "\r\n" + DownloadUrl32Bit : "";
            friendly += !string.IsNullOrEmpty(DownloadUrl32Bit) ? "\r\n" + DownloadUrl64Bit : "";
            return friendly;
        }

    }

    
}
