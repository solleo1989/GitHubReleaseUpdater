using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseUpdater.Models
{
    /// <summary>
    /// AssetModel for ReleaseModel
    /// </summary>
    public class AssetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //CreatedAt	{2022/5/11 19:38:18 +00:00}	System.DateTimeOffset
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset UpdatedTime { get; set; }
        public int Size { get; set; }
        public string Url { get; set; }
        public string DownloadUrl { get; set; }
    }
}
