// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommitRepository.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Repositories
{
    using System.Collections.Generic;
    using GitMonitor.Models;
    
    public interface ICommitRepository
    {
        IEnumerable<MonitoredItem> GetAll(bool includeAdvanced);

        IEnumerable<MonitoredItem> Get(string repoName);
    }
}
