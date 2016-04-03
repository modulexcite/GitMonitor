// --------------------------------------------------------------------------------------------------------------------
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

        public IEnumerable<MonitoredItem> GetAll()
        {
            try
            {
                List<MonitoredItem> xl = new List<MonitoredItem>();
                List<GitCommit> commits = new List<GitCommit>();
                DirectoryInfo di = new DirectoryInfo(Startup.Configuration["Repositories:Path"]);
                MonitoredItem mi = new MonitoredItem();
                foreach (var dir in di.GetDirectories())
                {
                    GitRepository gitrepo = new GitRepository { Name = dir.Name };
                    string commitUrl = Startup.Configuration[$"Repositories:{gitrepo.Name}"];

                    using (var repo = new Repository(dir.FullName))
                    {
                        DateTime startDate = DateTime.Now.AddDays(Convert.ToInt32(Startup.Configuration["Repositories:Age"]));
                        int commitCount = 0;
                        foreach (LibGit2Sharp.Commit com in repo.Head.Commits.Where(s => s.Committer.When >= startDate).OrderByDescending(s => s.Author.When))
                        {
                            // filter out merge commits
                            if (com.Parents.Count() > 1)
                            {
                                continue;
                            }

                            string url = string.IsNullOrWhiteSpace(commitUrl) ? string.Empty : string.Format($"{commitUrl}{com.Sha}");
                            
                            commits.Add(new GitCommit
                            {
                                Author = com.Author.Name,
                                AuthorEmail = com.Author.Email,
                                AuthorWhen = com.Author.When.UtcDateTime,
                                Committer = com.Committer.Name,
                                CommitterEmail = com.Committer.Email,
                                CommitterWhen = com.Committer.When.UtcDateTime,
                                Sha = com.Sha,
                                Message = com.Message,
                                RepositoryName = dir.Name,
                                RepositoryUrl = repo.Network.Remotes["origin"].Url,
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
