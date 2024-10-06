using eShopSolution.ViewModels.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Categories
{
	public class CategoryTreeModel
	{
		public string id { get; set; }
		public string text { get; set; }
		public string parent { set; get; }
	}
}
