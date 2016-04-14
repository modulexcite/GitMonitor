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

        public JsonResult Get()
        {
            var results = this.localRepository.GetAll(0);
            return this.Json(results);
        }

        [Route("{days:int}")]
        public JsonResult Get(int days)
        {
            var results = this.localRepository.GetAll(days);
            return this.Json(results);
        }

        //[Route("{repoName}")]
        //public JsonResult Get(string repoName)
        //{
        //    var results = this.localRepository.Get(repoName, "master", 0);
        //    return this.Json(results);
        //}


        //[Route("{repoName}")]
        //public JsonResult Get(string repoName, string branchName)
        //{
        //    var results = this.localRepository.Get(repoName, branchName, 0);
        //    return this.Json(results);
        //}

        //[Route("{repoName}/{days:int}")]
        //public JsonResult Get(string repoName, int days)
        //{
        //    var results = this.localRepository.Get(repoName, "master", days);
        //    return this.Json(results);
        //}

        [Route("{repoName}")]
        public JsonResult Get(string repoName, string branchName, int days)
        {
            if (branchName == null)
            {
                branchName = "master";
            }

            var results = this.localRepository.Get(repoName, branchName, days);
            return this.Json(results);
        }
    }
}