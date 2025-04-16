using GitHubReleaseUpdater.Models;
using Octokit;
using System.Linq;

namespace GitHubReleaseUpdater.Builders
{
    /// <summary>
    /// Build repository model including releases info fetched from GitHub
    /// </summary>
    public class RepoModelBuilder
    {
        UserModel User;
        RepoModel Model;
        ReleaseModelBuilder ReleaseBuilder;        

        public RepoModelBuilder(UserModel userModel) 
        {
            this.User = userModel;
            ReleaseBuilder = new ReleaseModelBuilder();
        }

        public RepoModel BuildRepoModel(string user, string repo)
        {
            GitHubClient client = ReleaseBuilder.ConnectGitHub(this.User);

            var model = new RepoModel(user, repo);
            model.Releases = ReleaseBuilder.GetAllRelease(client, user, repo);
            model.LatestRelease = model.Releases.FirstOrDefault();
            return model;
        }

    }
}
