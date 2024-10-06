using eShopSolution.ApiIntegration;
//using eShopSolution.Application.Utilities;
using eShopSolution.TechCare.Models;
using eShopSolution.TechCare.Utilities;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.TechCare.Controllers
{
	public class ProductCategoryController : Controller
	{
		private readonly IProductApiClient _productApiClient;
		private readonly ICategoryApiClient _categoryApiClient;
		private readonly IConfiguration _configuration;

		public ProductCategoryController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient, IConfiguration configuration)
		{
			_productApiClient = productApiClient;
			_categoryApiClient = categoryApiClient;
			_configuration = configuration;
		}


		[HttpGet]
		public async Task<IActionResult> Detail(GetManageProductPagingRequest request)
		{
			var categoryTreeAll = await _categoryApiClient.GetAll();
			CategoryViewModel productCategory = new CategoryViewModel();
			if (!string.IsNullOrEmpty(request.CategoryUrl))
			{
				productCategory = categoryTreeAll.Where(x => x.Alias == request.CategoryUrl).FirstOrDefault();
			}
			else if (request.CategoryId != null)
			{
				productCategory = categoryTreeAll.Where(x => x.Id == request.CategoryId).FirstOrDefault();
			}
			

			List<CategoryTreeNodeParent> listCategoryTreeById = new List<CategoryTreeNodeParent>();
			PagedResult<ProductViewModel> productRelation = new PagedResult<ProductViewModel>();
			List< CategoryViewModel> cateogoryChildHeaderSections = new List<CategoryViewModel>();

            if (productCategory != null)
			{
				var productCategoryTreeById = await _categoryApiClient.LoadCategoryTreeById(productCategory.Id);
                Utilities.Helper.BuildListCategoryTreebyId(productCategoryTreeById, ref listCategoryTreeById);
                cateogoryChildHeaderSections = await _categoryApiClient.LoadChildCategoryById(productCategory.Id);

                productRelation = await _productApiClient.GetPagings(
				   new GetManageProductPagingRequest()
				   {
					   PageIndex = request.PageIndex,
					   PageSize = request.PageSize,
					   CategoryUrl = request.CategoryUrl,
					   SortOption = request.SortOption,
					   Keyword = request.Keyword,
					   Purpose = request.Purpose,
					   ConditionPrice = request.ConditionPrice
				   });
			}

			var productViewModel = new ProductCategoryControllerViewModel()
			{
				ListCategoryTreeById = listCategoryTreeById,
				RelationProducts = productRelation,
				ProductRequest = request,
				CateogoryChildHeaderSections = cateogoryChildHeaderSections,
            };

            var categoryTreeGetAll = await _categoryApiClient.LoadCategoryTrees();
            ViewBag.CategoryTrees = categoryTreeGetAll;

			
            return View(productViewModel);
		}


		//#region FuncSupport
		//public void BuildListCategoryTreebyId(CategoryTreeNodeParent categoryTreeNode, ref List<CategoryTreeNodeParent> categoryTreeNodeParents)
		//{
		//	if (categoryTreeNode != null)
		//	{
		//		Console.WriteLine($"name: {categoryTreeNode.Name} - url: {categoryTreeNode.Alias}");
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
