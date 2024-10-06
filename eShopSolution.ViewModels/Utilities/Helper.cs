using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShopSolution.ViewModels.Utilities
{
    public class Helper
    {

        // Build list flat category tree
        public static void BuildListCategoryTreebyId(CategoryTreeNodeParent categoryTreeNode, ref List<CategoryTreeNodeParent> categoryTreeNodeParents)
        {
            if (categoryTreeNode != null)
            {
                Console.WriteLine($"name: {categoryTreeNode.Name} - url: {categoryTreeNode.Alias}");
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
