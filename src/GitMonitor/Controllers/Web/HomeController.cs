// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Mike Fourie">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace GitMonitor.Controllers
{
    using GitMonitor.Repositories;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> locallogger;
        private readonly ICommitRepository localRepository;

        public HomeController(ICommitRepository repository, ILogger<HomeController> logger)
        {
            this.localRepository = repository;
            this.locallogger = logger;
        }
        
        public IActionResult Index()
        {
            var results = this.localRepository.GetAll();
            return this.View(results);
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}
