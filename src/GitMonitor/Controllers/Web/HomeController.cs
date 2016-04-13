// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="FreeToDev">Mike Fourie</copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using GitMonitor.Models;
using Microsoft.Extensions.OptionsModel;

namespace GitMonitor.Controllers
{
    using GitMonitor.Repositories;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> locallogger;
        private readonly ICommitRepository localRepository;
        private IOptions<MonitoredPathConfig> localMonitoredPathConfig;

        public HomeController(ICommitRepository repository, ILogger<HomeController> logger, IOptions<MonitoredPathConfig> monitoredPathConfig)
        {
            this.localRepository = repository;
            this.locallogger = logger;
            this.localMonitoredPathConfig = monitoredPathConfig;
        }
        
        public IActionResult Index(int days)
        {

            var results = this.localRepository.GetDefault(this.localMonitoredPathConfig.Value, days);
            return this.View(results);
        }

        public IActionResult Fetch(int days)
        {
            this.localRepository.FetchAll();
            var results = this.localRepository.GetDefault(this.localMonitoredPathConfig.Value, days);
            return this.View(results);
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}
