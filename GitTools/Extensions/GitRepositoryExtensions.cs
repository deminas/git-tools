using GitTools.Models;
using System.IO;

namespace GitTools.Extensions
{
    internal static class GitRepositoryExtensions
    {
        public static bool Exists(this GitRepository repository)
        {
            return Directory.Exists(repository.Path);
        }
    }
}
