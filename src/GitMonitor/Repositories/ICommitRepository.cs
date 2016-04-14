// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommitRepository.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Repositories
{
    using System.Collections.Generic;
    using GitMonitor.Models;
    
    public interface ICommitRepository
    {
        IEnumerable<MonitoredItem> GetAll(int days);

        IEnumerable<MonitoredItem> Get(string repoName, string branchName, int days);
    }
}