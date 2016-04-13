// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitRepository.cs" company="FreeToDev">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------

using LibGit2Sharp;

namespace GitMonitor.Models
{
    public class GitRepository
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public int CommitCount { get; set; }

        public Repository Repository { get; set; }

    }
}
