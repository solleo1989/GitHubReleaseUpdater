using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubReleaseUpdater.Models
{
    /// <summary>
    ///  Model used for Github connection
    /// </summary>
    public class UserModel
    {
        public string UserName { get; set; }
        // A time based JWT token, signed by the GitHub App's private certificate
        public string UserToken { get; set; }

        public bool IsDummyUser => string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.UserToken);

        public UserModel() { }

        public UserModel(string userName, string userToken)
        {
            this.UserName = userName;
            this.UserToken = userToken;
        }
    }
}
