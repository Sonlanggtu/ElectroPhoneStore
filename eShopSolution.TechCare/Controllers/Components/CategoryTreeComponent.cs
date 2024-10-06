using eShopSolution.ApiIntegration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopSolution.TechCare.Controllers.Components
{

	public class CategoryTreeComponent : ViewComponent
	{
		private readonly ICategoryApiClient _categoryApiClient;

		public CategoryTreeComponent(ICategoryApiClient categoryApiClient)
		{
			_categoryApiClient = categoryApiClient;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categoryList = await _categoryApiClient.LoadCategoryTrees();

			return View("CategoryTreeComponent", categoryList);
		}
	}
}
