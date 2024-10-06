using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Utilities
{
	public class CategoryTreeNode
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alias { get; set; }
		public string Image { get; set; }
		public List<CategoryTreeNode> Children { get; set; } = new List<CategoryTreeNode>();

		public CategoryTreeNode(int id, string name, string alias, string image)
		{
			Id = id;
			Name = name;
			Alias = alias;
			Image = image;
		}

		public CategoryTreeNode()
		{
		}
	}
}
