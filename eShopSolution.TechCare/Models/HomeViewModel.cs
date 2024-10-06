using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static eShopSolution.Utilities.Constants.SystemConstants;

namespace eShopSolution.TechCare.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> FeaturedProducts { get; set; }
        public List<ProductViewModel> GamingProducts { get; set; }
        public List<ProductViewModel> GraphicsProducts { get; set; }
        public List<ProductViewModel> LuxuryProducts { get; set; }
        public List<ProductViewModel> OfficeProducts { get; set; }
        public List<ProductViewModel> AccessoriesProducts { get; set; }
        public List<ProductViewModel> MobileProducts { get; set; }
       
        
    }
}
