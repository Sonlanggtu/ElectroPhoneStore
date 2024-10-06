using eShopSolution.TechCare.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static eShopSolution.Utilities.Constants.SystemConstants;
using HtmlAgilityPack;

namespace eShopSolution.TechCare.Controllers
{
	//[Route("[controller]/[action]")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductApiClient _productApiClient;
		private readonly ICategoryApiClient _categoryApiClient;
		private readonly IConfiguration _configuration;

		public HomeController(ILogger<HomeController> logger,
			IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IConfiguration configuration)
		{
			_logger = logger;
			_productApiClient = productApiClient;
			_categoryApiClient = categoryApiClient;
			_configuration = configuration;

		}

		public async Task<IActionResult> Index()
		{
			var featuredProducts = await _productApiClient.GetFeaturedProducts(SystemConstants.ProductSettings.NumberOfFeturedProducts);
			var gamingProducts = await _productApiClient.GetPagings(
				new GetManageProductPagingRequest()
				{
					PageIndex = 1,
					PageSize = 10,
					Purpose = ProductConstants.LAPTOP_GAMING
				});

			var graphicsProducts = await _productApiClient.GetPagings(
				new GetManageProductPagingRequest()
				{
					PageIndex = 1,
					PageSize = 10,
					Purpose = ProductConstants.LAPTOP_GRAPHICS
				});

			var luxuryProducts = await _productApiClient.GetPagings(
				new GetManageProductPagingRequest()
				{
					PageIndex = 1,
					PageSize = 10,
					Purpose = ProductConstants.LAPTOP_LUXURY
				});

			var officeProducts = await _productApiClient.GetPagings(
			   new GetManageProductPagingRequest()
			   {
				   PageIndex = 1,
				   PageSize = 10,
				   Purpose = ProductConstants.LAPTOP_OFFFICE
			   });

			var accessoriesProducts = await _productApiClient.GetPagings(
				new GetManageProductPagingRequest()
				{
					PageIndex = 1,
					PageSize = 10,
					CategoryUrl = "phu-kien"
				});

			var viewModel = new HomeViewModel
			{
				FeaturedProducts = featuredProducts,
				GamingProducts = gamingProducts.Items,
				GraphicsProducts = graphicsProducts.Items,
				LuxuryProducts = luxuryProducts.Items,
				AccessoriesProducts = accessoriesProducts.Items,
				OfficeProducts = officeProducts.Items
			};

			var productCategoryTreeById = await _categoryApiClient.LoadCategoryTrees();

			ViewBag.CategoryTrees = productCategoryTreeById;

			return View(viewModel);
		}

		[HttpGet]
		public async Task<IActionResult> ViewByCategory(string sortOrder, int cateId, int pageIndex = 1, int pageSize = 8)
		{
			var request = new GetManageProductPagingRequest()
			{
				PageIndex = pageIndex,
				PageSize = pageSize,
				CategoryId = cateId,
				SortOption = sortOrder
			};

			var data = await _productApiClient.GetPagings(request);

			List<string> sortOption = new List<string>()
			{
				"Tên A-Z",
				"Giá thấp đến cao",
				"Giá cao đến thấp"
			};

			ViewBag.SortOption = sortOption;
			ViewBag.CurrentSortOrder = sortOrder;

			foreach (var item in data.Items)
			{
				var category = await _categoryApiClient.GetById(item.CategoryId);
				item.Category = category;
			}

			return View(data);
		}

		[HttpGet]
		public async Task<IActionResult> ViewBySearchProduct(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 8)
		{
			var request = new GetManageProductPagingRequest()
			{
				Keyword = keyword,
				PageIndex = pageIndex,
				PageSize = pageSize,
				CategoryId = categoryId
			};

			var data = await _productApiClient.GetPagings(request);
			ViewBag.Keyword = keyword;

			foreach (var item in data.Items)
			{
				var category = await _categoryApiClient.GetById(item.CategoryId);
				item.Category = category;
			}

			if (TempData["result"] != null)
			{
				ViewBag.SuccessMsg = TempData["result"];
			}
			return View(data);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		public IActionResult SetCultureCookie(string cltr, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
				);

			return LocalRedirect(returnUrl);
		}

		// Hàm giải mã token ( chứa thông tin về đăng nhập )
		private ClaimsPrincipal ValidateToken(string jwtToken)
		{
			Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

			SecurityToken validatedToken;
			TokenValidationParameters validationParameters = new TokenValidationParameters();

			validationParameters.ValidateLifetime = true;

			validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
			validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
			validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));


			// Giải mã thông tin claim mà ta đã gắn cho token ấy ( định nghĩa claim trong UserService.cs )
			ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

			// trả về một principal có token đã giải mã
			return principal;
		}
	}
}
