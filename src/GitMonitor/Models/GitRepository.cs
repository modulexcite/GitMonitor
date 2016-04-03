// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitRepository.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Models
{
    public class GitRepository
    {
        public string Name { get; set; }

        public int CommitCount { get; set; }
    }
}
