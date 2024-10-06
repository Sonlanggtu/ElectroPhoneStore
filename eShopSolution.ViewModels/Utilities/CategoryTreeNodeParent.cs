using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Utilities
{
	public class CategoryTreeNodeParent
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public CategoryTreeNodeParent Parent { get; set; } //= new CategoryTreeNodeParent();

		public CategoryTreeNodeParent(int id, string name, string alias)
		{
			Id = id;
			Name = name;
			Alias = alias;
		}

		public CategoryTreeNodeParent()
		{
		}
	}
}
