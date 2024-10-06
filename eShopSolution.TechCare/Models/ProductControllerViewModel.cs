using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Utilities;
using System.Collections.Generic;

namespace eShopSolution.TechCare.Models
{
    public class ProductControllerViewModel
    {
        public ProductViewModel ProductViewModel { get; set; }
        public List<CategoryTreeNodeParent> ListCategoryTreeById { get; set; }
        public List<ProductViewModel> RelationProducts { get; set; }
    }
}
