﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommitRepository.cs" company="FreeToDev">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Repositories
{
    using GitMonitor.Models;

    public interface ICommitRepository
    {
        void FetchAll(MonitoredPathConfig m, string name);

        MonitoredPathConfig Get(MonitoredPathConfig m, string name, string branchName, int days);

        MonitoredPath Get(MonitoredPathConfig m, string pathName, string repoName, string branchName, int days);
    }
}
