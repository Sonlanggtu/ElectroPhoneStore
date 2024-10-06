using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace eShopSolution.TechCare.Controllers
{
    //[Route("[controller]/[action]")]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IConfiguration _configuration;

        public PostController(ILogger<PostController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }
    }
}

