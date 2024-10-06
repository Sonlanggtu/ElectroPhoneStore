using eShopSolution.ViewModels.Utilities;
using System.Collections.Generic;

namespace eShopSolution.TechCare.Utilities
{
	public class Helper
	{
		public static void BuildListCategoryTreebyId(CategoryTreeNodeParent categoryTreeNode, ref List<CategoryTreeNodeParent> categoryTreeNodeParents)
		{
			if (categoryTreeNode != null)
			{
				categoryTreeNodeParents.Add(
						new CategoryTreeNodeParent()
						{
							Id = categoryTreeNode.Id,
							Name = categoryTreeNode.Name,
							Alias = categoryTreeNode.Alias
						});
				BuildListCategoryTreebyId(categoryTreeNode.Parent, ref categoryTreeNodeParents);
			}
		}
	}
}
