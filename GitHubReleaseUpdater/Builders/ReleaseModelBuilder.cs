using GitHubReleaseUpdater.Models;
using Octokit;
using System;
using System.Collections.Generic;

namespace GitHubReleaseUpdater.Builders
{
    public class ReleaseModelBuilder
    {
        private string ErrorMessage = "";

        public IEnumerable<ReleaseModel> GetAllRelease(GitHubClient client, string user, string repo)
        {            
            try
            {
                // Fetch all releases from repo using given user name nad repo
                var results = new List<ReleaseModel>();
                var releases = client.Repository?.Release?.GetAll(user, repo)?.Result;
                if (releases != null)
                {
                    var count = 0;
                    foreach (var release in releases)
                    {
                        var releaseModel = BuildReleaseModel(release);
                        releaseModel.Index = releases.Count - count;

                        results.Add(releaseModel);
                        count++;
                    }
                }
                return results;
            }
            catch (Exception e)
            { 
                return new List<ReleaseModel>();
            }

        }

        /// <summary>
        /// Fucntion to fetch the latest release information instead of whole. Depreciated.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="user"></param>
        /// <param name="repo"></param>
        /// <returns></returns>
        public ReleaseModel GetLatestRelease(GitHubClient client, string user, string repo)
        {
            var latestRelease = client.Repository?.Release?.GetLatest(user, repo)?.Result;
            Console.WriteLine(
                "The latest release is tagged at {0} and is named {1}",
                latestRelease.TagName,
                latestRelease.Name);
            
            return latestRelease != null ? BuildReleaseModel(latestRelease) : new ReleaseModel();
        }

        /// <summary>
        /// Function to transfer Release model to a local ReleaseModel.
        /// </summary>
        /// <param name="model">a OctoKit Release model fetched from GitHub Repo.</param>
        /// <returns></returns>
        public ReleaseModel BuildReleaseModel(Release model)
        {
            var releaseModel = new ReleaseModel 
            { 
                Id = model.Id,
                Name = model.TagName,
                IsPreRelease = model.Prerelease,
                HtmlUrl = model.HtmlUrl,
                CreatedTime = model.CreatedAt,
                ZipFileUrl = model.ZipballUrl,
                DescriptionShort = model.Name,
                Assets = new List<AssetModel>()
            };

            foreach (var asset in model.Assets)
            {
                var assetModel = new AssetModel()
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Url = asset.Url,
                    CreatedTime = asset.CreatedAt,
                    UpdatedTime = asset.UpdatedAt,
                    Size = asset.Size,
                    DownloadUrl = asset.BrowserDownloadUrl,
                };
                releaseModel.Assets.Add(assetModel);
            }

            return releaseModel;
        }

        public GitHubClient ConnectGitHub(UserModel user)
        {
            // Use the JWT as a Bearer token
            var client = new GitHubClient(new ProductHeaderValue(user.UserName))
            {
                // A time based JWT token, signed by the GitHub App's private certificate
                Credentials = new Credentials(user.UserToken, AuthenticationType.Oauth)
            };

            return client;
        }

        private bool DefaultIsStable(Dictionary<string, object> release) => isValidVersion(release);

        private bool DefaultIsBeta(Dictionary<string, object> release)
        {
            return release["name"].ToString().ToLower().EndsWith("(beta)");
        }
        private bool isValidVersion(Dictionary<string, object> release)
        {
            try
            {
                new Version(release["tag_name"].ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AddErrorMessage(string err)
        {
            if (ErrorMessage != "")
                ErrorMessage += "\r\n";
            ErrorMessage += err;
        }
    }
}
