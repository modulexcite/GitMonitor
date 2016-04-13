// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitoredItem.cs" company="FreeToDev">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Models
{
    using System.Collections.Generic;

    public class MonitoredPath
    {
        public MonitoredPath()
        {
            this.Commits = new List<GitCommit>();
            this.Repositories = new List<GitRepository>();
        }

        public string Name { get; set; }

        public List<GitRepository> Repositories { get; set; }

        public List<GitCommit> Commits { get; set; }
    }
}
