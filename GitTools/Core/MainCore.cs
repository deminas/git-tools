using GitTools.Core.BaseCore;
using GitTools.Models;
using LibGit2Sharp;

namespace GitTools.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// https://github.com/libgit2/libgit2sharp/wiki/git-branch
    /// </remarks>
    internal class MainCore : BaseSingletonCore
    {
        private RepositoriesListCore _repositories;

        public List<GitRepository> Repositories => _repositories.List;

        public List<GitBranch> Branches { get; private set; } = new List<GitBranch>();

        public MainCore(RepositoriesListCore repositories)
        {
            _repositories = repositories;
        }

        //private GitBranch[] GetBranches()
        //{
        //    return _branchHelper.GetLocalBranches("D:\\Projects\\VisualStudio\\Work\\reportbi").ToArray();
        //}


        public void AddTag(string path, string tag)
        {
            using var repo = new Repository(path);
            repo.ApplyTag("v0.1");

        }
    }
}
