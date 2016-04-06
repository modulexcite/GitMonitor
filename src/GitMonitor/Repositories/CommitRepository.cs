﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommitRepository.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GitMonitor.Models;
    using LibGit2Sharp;
    using Microsoft.Extensions.Logging;

    public class CommitRepository : ICommitRepository
    {
        private readonly ILogger<CommitRepository> locallogger;

        public CommitRepository(ILogger<CommitRepository> logger)
        {
            this.locallogger = logger;
        }

        public IEnumerable<MonitoredItem> GetAll(bool includeAdvanced)
        {
            try
            {
                List<MonitoredItem> xl = new List<MonitoredItem>();
                DirectoryInfo di = new DirectoryInfo(Startup.Configuration["Repositories:DefaultPath"]);                
                xl.Add(this.GetMonitoredItem(di, "Default"));
                if (includeAdvanced)
                {
                    string[] paths = Startup.Configuration["Repositories:AdvancedPaths"].Split(',');
                    foreach (string s in paths)
                    {
                        DirectoryInfo d = new DirectoryInfo(s);
                        xl.Add(this.GetMonitoredItem(d, d.Name));
                    }
                }
                return xl;
            }
            catch (Exception ex)
            {
                this.locallogger.LogError("Bad - ", ex);
                return null;
            }
        }

        private MonitoredItem GetMonitoredItem(DirectoryInfo di, string itemName)
        {
            List<GitCommit> commits = new List<GitCommit>();
            MonitoredItem mi = new MonitoredItem { Name = itemName };
            foreach (var dir in di.GetDirectories())
            {
                GitRepository gitrepo = new GitRepository { Name = dir.Name.Replace(".git", string.Empty) };

                string commitUrl = Startup.Configuration[$"Repositories:{gitrepo.Name}"];
                string repoFriendlyName = Startup.Configuration[$"Repositories:{gitrepo.Name}FriendlyName"];
                repoFriendlyName = string.IsNullOrWhiteSpace(repoFriendlyName) ? dir.Name : repoFriendlyName;
                gitrepo.FriendlyName = repoFriendlyName;
                using (var repo = new Repository(dir.FullName))
                {
                    DateTime startDate = DateTime.Now.AddDays(Convert.ToInt32(Startup.Configuration["Repositories:DefaultAge"]));
                    int commitCount = 0;
                    string branch = repo.Info.IsBare ? "master" : "origin/master";
                    foreach (
                        LibGit2Sharp.Commit com in
                            repo.Branches[branch].Commits.Where(s => s.Committer.When >= startDate)
                                .OrderByDescending(s => s.Author.When))
                    {
                        // filter out merge commits
                        if (com.Parents.Count() > 1)
                        {
                            continue;
                        }

                        string[] nameexclusions =
                            Startup.Configuration["Repositories:UserNameExcludeFilter"].Split(',');
                        if (nameexclusions.Any(name => com.Author.Name.Contains(name)))
                        {
                            continue;
                        }

                        string url = string.IsNullOrWhiteSpace(commitUrl)
                            ? string.Empty
                            : string.Format($"{commitUrl}{com.Sha}");
                        string repositoryUrl = string.Empty;
                        if (repo.Network.Remotes?["origin"] != null)
                        {
                            repositoryUrl = repo.Network.Remotes["origin"].Url;
                        }

                        commits.Add(new GitCommit
                        {
                            Author = com.Author.Name,
                            AuthorEmail = string.IsNullOrWhiteSpace(com.Author.Email) ? string.Empty : com.Author.Email,
                            AuthorWhen = com.Author.When.UtcDateTime,
                            Committer = com.Committer.Name,
                            CommitterEmail = string.IsNullOrWhiteSpace(com.Committer.Email) ? string.Empty : com.Committer.Email,
                            CommitterWhen = com.Committer.When.UtcDateTime,
                            Sha = com.Sha,
                            Message = com.Message,
                            RepositoryFriendlyName = repoFriendlyName,
                            RepositoryName = dir.Name,
                            RepositoryUrl = repositoryUrl,
                            CommitUrl = url
                        });
                        commitCount++;
                    }

                    gitrepo.CommitCount = commitCount;
                    mi.Repositories.Add(gitrepo);
                }

                mi.Commits = commits;
                mi.Commits.Sort((x, y) => -DateTime.Compare(x.CommitterWhen, y.CommitterWhen));
            }

            return mi;
        }

        public IEnumerable<MonitoredItem> Get(string repoName)
        {
            try
            {
                List<MonitoredItem> xl = new List<MonitoredItem>();
                List<GitCommit> commits = new List<GitCommit>();
                MonitoredItem mi = new MonitoredItem();
                DirectoryInfo dir = new DirectoryInfo(Startup.Configuration["Repositories:Path"] + @"\" + repoName);
                GitRepository gitrepo = new GitRepository { Name = dir.Name };

                string commitUrl = Startup.Configuration[$"Repositories:{gitrepo.Name}"];
                string repoFriendlyName = Startup.Configuration[$"Repositories:{gitrepo.Name}FriendlyName"];
                repoFriendlyName = string.IsNullOrWhiteSpace(repoFriendlyName) ? dir.Name : repoFriendlyName;
                gitrepo.FriendlyName = repoFriendlyName;
                using (var repo = new Repository(dir.FullName))
                {
                    DateTime startDate = DateTime.Now.AddDays(Convert.ToInt32(Startup.Configuration["Repositories:Age"]));
                    int commitCount = 0;
                    string branch = repo.Info.IsBare ? "master" : "origin/master";
                    foreach (LibGit2Sharp.Commit com in
                            repo.Branches[branch].Commits.Where(s => s.Committer.When >= startDate)
                                .OrderByDescending(s => s.Author.When))
                    {
                        // filter out merge commits
                        if (com.Parents.Count() > 1)
                        {
                            continue;
                        }

                        string[] nameexclusions = Startup.Configuration["Repositories:UserNameExcludeFilter"].Split(',');
                        if (nameexclusions.Any(name => com.Author.Name.Contains(name)))
                        {
                            continue;
                        }

                        string url = string.IsNullOrWhiteSpace(commitUrl)
                            ? string.Empty
                            : string.Format($"{commitUrl}{com.Sha}");
                        string repositoryUrl = string.Empty;
                        if (repo.Network.Remotes?["origin"] != null)
                        {
                            repositoryUrl = repo.Network.Remotes["origin"].Url;
                        }

                        commits.Add(new GitCommit
                        {
                            Author = com.Author.Name,
                            AuthorEmail = string.IsNullOrWhiteSpace(com.Author.Email) ? string.Empty : com.Author.Email,
                            AuthorWhen = com.Author.When.UtcDateTime,
                            Committer = com.Committer.Name,
                            CommitterEmail =
                                string.IsNullOrWhiteSpace(com.Committer.Email) ? string.Empty : com.Committer.Email,
                            CommitterWhen = com.Committer.When.UtcDateTime,
                            Sha = com.Sha,
                            Message = com.Message,
                            RepositoryFriendlyName = repoFriendlyName,
                            RepositoryName = dir.Name,
                            RepositoryUrl = repositoryUrl,
                            CommitUrl = url
                        });
                        commitCount++;
                    }

                    gitrepo.CommitCount = commitCount;
                    mi.Repositories.Add(gitrepo);
                }

                mi.Commits = commits;
                mi.Commits.Sort((x, y) => -DateTime.Compare(x.CommitterWhen, y.CommitterWhen));
                xl.Add(mi);
                return xl;
            }
            catch (Exception ex)
            {
                this.locallogger.LogError("Bad - ", ex);
                return null;
            }
        }
    }
}
