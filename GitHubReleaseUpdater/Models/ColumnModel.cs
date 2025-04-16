using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitHubReleaseUpdater.Models
{
    /// <summary>
    /// Depricated.
    /// </summary>
    public class ColumnModel
    {
        public DataGridViewColumn UserColumn { get; set; }
        public DataGridViewColumn RepositoryColumn { get; set; }
        public DataGridViewColumn CategoryColumn { get; set; }
        public DataGridViewColumn LocalPathColumn { get; set; }
        public DataGridViewColumn CurrentReleaseColumn { get; set; }
        public DataGridViewColumn LatestReleaseColumn { get; set; }

        public ColumnModel() 
        { 

        }
    }
}
