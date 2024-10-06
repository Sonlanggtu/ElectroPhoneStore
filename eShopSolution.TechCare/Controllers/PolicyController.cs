using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eShopSolution.TechCare.Controllers
{
	//[Route("[controller]/[action]")]
	public class PolicyController : Controller
    {
        private readonly ILogger<PolicyController> _logger;
        private readonly IConfiguration _configuration;

        public PolicyController(ILogger<PolicyController> logger,  IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Security()
        {
            return View();
        }

        public IActionResult Warranty()
        {
            return View();
        }

        public IActionResult Return()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
