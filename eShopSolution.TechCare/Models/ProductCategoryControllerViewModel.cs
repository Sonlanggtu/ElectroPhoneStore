using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities;
using System.Collections.Generic;

namespace eShopSolution.TechCare.Models
{
	public class ProductCategoryControllerViewModel
	{
		public List<CategoryTreeNodeParent> ListCategoryTreeById { get; set; }
		public PagedResult<ProductViewModel> RelationProducts { get; set; }
		public GetManageProductPagingRequest ProductRequest { get; set; }
        public List<CategoryViewModel> CateogoryChildHeaderSections { get; set; }
        

    }
}
