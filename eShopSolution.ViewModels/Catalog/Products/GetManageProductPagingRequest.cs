using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryUrl { get; set; }
        public int Purpose { set; get; }

        public string? SortOption { get; set; }
		public string? ConditionPrice { get; set; }
	}
}