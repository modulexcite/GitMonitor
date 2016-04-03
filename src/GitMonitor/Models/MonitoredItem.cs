// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitoredItem.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Models
{
    using System.Collections.Generic;

    public class MonitoredItem
    {
        public MonitoredItem()
        {
            this.Commits = new List<GitCommit>();
            this.Repositories = new List<GitRepository>();
        }

        public List<GitRepository> Repositories { get; set; }

        public List<GitCommit> Commits { get; set; }
    }
}
