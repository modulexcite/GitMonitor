// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitController.cs" company="FreeToDev">Copyright Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace GitMonitor.Controllers
{
    using GitMonitor.Repositories;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/commits")]
    public class GitController : Controller
    {
        private readonly ILogger<GitController> locallogger;
        private readonly ICommitRepository localRepository;

        public GitController(ICommitRepository repository, ILogger<GitController> logger)
        {
            this.localRepository = repository;
            this.locallogger = logger;
        }

        public JsonResult Get()
        {
            int days = Convert.ToInt32(Startup.Configuration["Repositories:DefaultDays"]);
            if (days == 0)
            {
                days = -1;
            }

            if (days > 0)
            {
                days = days * -1;
            }

            var results = this.localRepository.GetDefault(null, days);
            return this.Json(results);
        }

        [Route("{days:int}")]
        public JsonResult Get(int days)
        {
            var results = this.localRepository.GetDefault(null, days);
            return this.Json(results);
        }

        [Route("{repoName}")]
        public JsonResult Get(string repoName)
        {
            var results = this.localRepository.Get(repoName, 0);
            return this.Json(results);
        }

        [Route("{repoName}/{days:int}")]
        public JsonResult Get(string repoName, int days)
        {
            var results = this.localRepository.Get(repoName, days);
            return this.Json(results);
        }
    }
}