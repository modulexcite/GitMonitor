// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitController.cs" company="Mike Fourie">Copyright Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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

        [HttpGet("")]
        public JsonResult Get()
        {
            var results = this.localRepository.GetAll();
            return this.Json(results);
        }
    }
}
