using GitTools.Configs;
using GitTools.Core.BaseCore;
using GitTools.Models;

namespace GitTools.Core
{
    internal class RepositoriesListCore : BaseSingletonCore
    {
        private const string RepositoryName = "Repositories";

        public readonly AppConfig _config;

        public List<GitRepository> List { get; private set; } = new List<GitRepository>();

        public RepositoriesListCore(AppConfig config)
        {
            _config = config;
            Open();
        }

        public void Open()
        {
            List = _config.Open<List<GitRepository>>(RepositoryName) ?? new List<GitRepository>();

        }

        public void Save()
        {
            _config.Save(RepositoryName, List);
        }
    }
}
