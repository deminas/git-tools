using GitTools.Core.BaseCore;
using GitTools.Models;
using LibGit2Sharp;

namespace GitTools.Core
{
    internal class RepositoryCore : BaseTransientCore
    {
        public GitRepository? Repository { get; set; }

        public List<GitBranch> Branches { get; private set; } = new List<GitBranch>();

        public RepositoryCore()
        {
        }

        public void RefreshBranches()
        {
            if (Repository is null)
            {
                throw new ArgumentNullException(nameof(Repository));
            }

            using var repo = new Repository(Repository?.Path);

            Branches = repo.Branches
                .Where(b => !b.IsRemote)
                .Select(b => new GitBranch
                {
                    Name = b.FriendlyName,
                    IsHead = b.IsCurrentRepositoryHead
                })
                .ToList();
        }

        public void CreateBranch(string path, string branchName)
        {
            using var repo = new Repository(path);
            repo.CreateBranch(branchName);
        }

        public void CreateBranch(string path, string branchName, string commit)
        {
            using var repo = new Repository(path);
            repo.CreateBranch(branchName);
            repo.Branches.Add(branchName, commit);
        }

        public void DeleteBranch(string path, string branchName)
        {
            using var repo = new Repository(path);
            repo.Branches.Remove(branchName);
        }

        public void EditDescriptionBranch(string path, string branchName, string description)
        {
            using var repo = new Repository(path);

            //var key = string.Format("branch.{0}.description", branch);
            //repo.Config.Set(key, description.Replace(Environment.NewLine, string.Empty)); // set description
            //repo.Config.Unset(key); // remove description

        }

        public void Merge(string path, string fromBranchName, string toBranchName, string author, string email)
        {
            using var repo = new Repository(path);
            var toBranch = repo.Branches[toBranchName];
            var fromBranch = repo.Branches[fromBranchName];
            var merger = new Signature(author, email, DateTime.Now);

            Commands.Checkout(repo, toBranch);
            repo.Merge(fromBranch, merger, new MergeOptions
            {
                FastForwardStrategy = FastForwardStrategy.NoFastForward
            });


            //Commands.Checkout(repo, developBranch);
            //repo.Merge(toBranch, merger, new MergeOptions());
        }
    }
}
