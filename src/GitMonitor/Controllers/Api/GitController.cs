// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitController.cs" company="Mike Fourie">Copyright Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
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

        [Route("{repoName}")]
        public JsonResult Get(string repoName)
        {
            var results = this.localRepository.Get(repoName);
            return this.Json(results);
        }

        public JsonResult Get()
        {
            var results = this.localRepository.GetAll(false);
            return this.Json(results);
        }
    }
}