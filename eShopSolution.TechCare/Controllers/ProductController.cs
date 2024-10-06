using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.TechCare.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static eShopSolution.Utilities.Constants.SystemConstants;
using eShopSolution.ViewModels.Utilities;
using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.Extensions.Configuration;
using eShopSolution.TechCare.Utilities;
//using eShopSolution.Application.Utilities;

namespace eShopSolution.TechCare.Controllers
{
	//[Route("[controller]/[action]")]
	public class ProductController : Controller
	{
		private readonly IProductApiClient _productApiClient;
		private readonly ICategoryApiClient _categoryApiClient;
		private readonly IUserApiClient _userApiClient;
		private readonly IConfiguration _configuration;

		public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IUserApiClient userApiClient, IConfiguration configuration)
		{
			_productApiClient = productApiClient;
			_categoryApiClient = categoryApiClient;
			_userApiClient = userApiClient;
			_configuration = configuration;
		}


		public IActionResult Index(string urlProduct)
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Detail(string urlProduct, int idProduct)
		{
			var product = await _productApiClient.GetById(idProduct);
			var productCategorybyId = await _categoryApiClient.LoadCategoryTreeById(product.CategoryId);

			//var productCategory = await _categoryApiClient.LoadCategoryTrees();
			//gencontent(productCategorybyId);

			var productRelation = await _productApiClient.GetPagings(
				new GetManageProductPagingRequest()
				{
					PageIndex = 1,
					PageSize = 11,
					Purpose = product.Purpose
				});
			productRelation.Items = productRelation.Items.Where(x => x.Id != idProduct).ToList();

			List<CategoryTreeNodeParent> listCategoryTreeById = new List<CategoryTreeNodeParent>();
            Utilities.Helper.BuildListCategoryTreebyId(productCategorybyId, ref listCategoryTreeById);

			var productViewModel = new ProductControllerViewModel()
			{
				ProductViewModel = product,
				ListCategoryTreeById = listCategoryTreeById,
				RelationProducts = productRelation.Items,
			};

            var productCategoryTreeById = await _categoryApiClient.LoadCategoryTrees();
            ViewBag.CategoryTrees = productCategoryTreeById;
            return View(productViewModel);
		}



		//public async Task<IActionResult> Detail(int id)
		//{
		//    var product = await _productApiClient.GetById(id);
		//    ViewBag.ProductId = id;

		//    var reviews = product.Reviews;
		//    ViewBag.Comments = reviews;

		//    var ratings = product.Reviews;
		//    if (ratings.Count() > 0)
		//    {
		//        var ratingSum = ratings.Sum(d => d.Rating);
		//        ViewBag.RatingSum = ratingSum;
		//        var ratingCount = ratings.Count();
		//        ViewBag.RatingCount = ratingCount;
		//    }
		//    else
		//    {
		//        ViewBag.RatingSum = 0;
		//        ViewBag.RatingCount = 0;
		//    }

		//    var category = await _categoryApiClient.GetById(product.CategoryId);

		//    var productDetailViewModel = new ProductDetailViewModel()
		//    {
		//        Category = category,
		//        Product = product,
		//        ListOfReviews = reviews
		//    };

		//    // get user review name
		//    foreach (var review in productDetailViewModel.ListOfReviews)
		//    {
		//        Guid userId = new Guid(review.UserId.ToString());
		//        var user = await _userApiClient.GetById(userId);
		//        review.UserName = user.ResultObj.Name;
		//    }

		//    return View(productDetailViewModel);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<ActionResult> AddReview(ProductDetailViewModel model)
		//{
		//    var productDetailVM = new ProductDetailViewModel()
		//    {
		//        ProductId = model.ProductId,
		//        Review = model.Review,
		//        Rating = model.Rating,
		//        UserCommentId = model.UserCommentId
		//    };

		//    var request = await _productApiClient.AddReview(productDetailVM);

		//    int Id = int.Parse(request);

		//    return RedirectToAction("Detail", "Product", new { id = Id });
		//}

		//public async Task<IActionResult> Category(int id, string culture, int page = 1)
		//{
		//	var products = await _productApiClient.GetPagings(new GetManageProductPagingRequest()
		//	{
		//		CategoryId = id,
		//		PageIndex = page,
		//		PageSize = 10
		//	});
		//	return View(new ProductCategoryViewModel()
		//	{
		//		Category = await _categoryApiClient.GetById(id),
		//		Products = products
		//	});
		//}

		//#region FuncSupport
		//public void BuildListCategoryTreebyId(CategoryTreeNodeParent categoryTreeNode, ref List<CategoryTreeNodeParent> categoryTreeNodeParents)
		//{
		//	if (categoryTreeNode != null)
		//	{
		//		//Console.WriteLine($"name: {categoryTreeNode.Name} - url: {categoryTreeNode.Alias}");
		//		categoryTreeNodeParents.Add(
		//				new CategoryTreeNodeParent()
		//				{
		//					Id = categoryTreeNode.Id,
		//					Name = categoryTreeNode.Name,
		//					Alias = categoryTreeNode.Alias
		//				});
		//		BuildListCategoryTreebyId(categoryTreeNode.Parent, ref categoryTreeNodeParents);
		//	}
		//}
		//#endregion
	}
}