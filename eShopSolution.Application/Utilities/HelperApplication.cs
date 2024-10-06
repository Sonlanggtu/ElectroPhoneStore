using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eShopSolution.Application.Utilities
{
	public class HelperApplication
	{

	}


	public class CategoryTreeHelper
	{

		// Xây dựng cây thư mục từ danh sách các danh mục
		public static List<CategoryTreeNode> BuildCategoryTree(List<Category> categories)
		{
			// Tạo danh sách các danh mục gốc (idParent = 0)
			var rootCategories = categories.Where(c => c.idParent == 0).ToList();
			List<CategoryTreeNode> categoryTree = new List<CategoryTreeNode>();

			if (rootCategories != null && rootCategories.Any())
			{
				foreach (var rootCategory in rootCategories)
				{
					var rootNode = new CategoryTreeNode(rootCategory.Id, rootCategory.Name, rootCategory.Alias, rootCategory.Image);
					rootNode.Children = BuildSubCategories(rootCategory.Id, categories);
					categoryTree.Add(rootNode);
				}
			}
			return categoryTree;
		}

		// Hàm đệ quy để xây dựng các danh mục con
		private static List<CategoryTreeNode> BuildSubCategories(int parentId, List<Category> categories)
		{
			var subCategories = categories.Where(c => c.idParent == parentId).ToList();
			List<CategoryTreeNode> subCategoryNodes = new List<CategoryTreeNode>();

			foreach (var subCategory in subCategories)
			{
				var subNode = new CategoryTreeNode(subCategory.Id, subCategory.Name, subCategory.Alias, subCategory.Image);
				subNode.Children = BuildSubCategories(subCategory.Id, categories); // Đệ quy để tìm các con của danh mục hiện tại
				subCategoryNodes.Add(subNode);
			}

			return subCategoryNodes;
		}


		// Build categoryTree by CategoryId (from parent search child)
		// Xây dựng cây thư mục từ danh sách các danh mục
		public static CategoryTreeNode BuildCategoryTree(int? cateogortId, string cateogoryAlias, List<Category> categories)
		{
			var rootCategory = new Category();
			if (cateogortId != null && cateogortId != 0)
			{
				rootCategory = categories.Where(c => c.Id == cateogortId).FirstOrDefault();
			}
			else
			{
				rootCategory = categories.Where(c => c.Alias == cateogoryAlias).FirstOrDefault();
			}

			CategoryTreeNode categoryTree = new CategoryTreeNode();
			if (rootCategory != null)
			{
				var rootNode = new CategoryTreeNode(rootCategory.Id, rootCategory.Name, rootCategory.Alias, rootCategory.Image);
				categoryTree = rootNode;
				rootNode.Children = BuildSubCategories(rootCategory.Id, categories);
			}
			return categoryTree;
		}


		// Build categoryTree by CategoryId (from child seach parent)
		public static CategoryTreeNodeParent BuildCategoryTreeById(List<Category> categoriesAll, int categoryId)
		{
			try
			{
				var category = categoriesAll.Where(c => c.Id == categoryId).FirstOrDefault();
				if (category != null)
				{
					CategoryTreeNodeParent categoryTree = new CategoryTreeNodeParent();
					var chilrenNode = new CategoryTreeNodeParent(category.Id, category.Name, category.Alias);
					categoryTree = chilrenNode;
					categoryTree.Parent = BuildParentCategory(category.idParent, categoriesAll);
					return categoryTree;
				}
				else
				{
					return null;
				}

			}
			catch (Exception ex)
			{
				return null;
			}

		}


		private static CategoryTreeNodeParent BuildParentCategory(int childrenId, List<Category> categoryAll)
		{
			var categoryParent = categoryAll.Where(c => c.Id == childrenId).FirstOrDefault();

			if (categoryParent != null)
			{
				CategoryTreeNodeParent subCategoryNodes = new CategoryTreeNodeParent();
				var subNode = new CategoryTreeNodeParent(categoryParent.Id, categoryParent.Name, categoryParent.Alias);
				subCategoryNodes = subNode;
				subCategoryNodes.Parent = BuildParentCategory(categoryParent.idParent, categoryAll); // Đệ quy để tìm các con của danh mục hiện tại
				return subCategoryNodes;
			}
			else
			{
				return null;
			}
		}


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
