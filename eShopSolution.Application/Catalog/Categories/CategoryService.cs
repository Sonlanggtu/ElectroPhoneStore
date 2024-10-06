using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Categories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.Application.Utilities;
using eShopSolution.ViewModels.Utilities;
using System;
using Org.BouncyCastle.Asn1.Ocsp;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using eShopSolution.Application.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
		private readonly IStorageService _storageService;
		private readonly IConfiguration _configuration;
		private const string USER_CONTENT_FOLDER_NAME = "user-content";
		public CategoryService(EShopDbContext context, IConfiguration configuration, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
            _configuration = configuration;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                idParent = request.idParent,
                Alias = request.Alias,
                isEnable = true
            };

			//Save icon image
			if (request.Image != null)
			{
				category.Image = await this.SaveFile(request.Image);
			}
			_context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> Update(CategoryUpdateRequest request)
        {
            try
            {
				var category = await _context.Categories.FindAsync(request.Id);
				if (category == null) throw new EShopException($"Không thể tìm danh mục có ID: {request.Id} ");

				category.Name = request.Name;
				category.idParent = request.IdParent;
				category.Alias = request.Alias;


				//Save Icon image
				string image = string.Empty;
				if (request.Image != null)
				{
					image = await this.SaveFile(request.Image);
				}
				if (request.Image == null && !string.IsNullOrEmpty(request.ImageSavedStr)) // images has been saved
				{
					image = request.ImageSavedStr;
				}
				category.Image = image;

				return await _context.SaveChangesAsync();
			}
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<int> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null) throw new EShopException($"Không thể tìm danh mục có ID: {categoryId} ");

            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var query = from c in _context.Categories
                        select new { c };

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.c.Name.Contains(request.Keyword));

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.c.Id,
                    Name = x.c.Name,
                    idParent = x.c.idParent,
                    Alias = x.c.Alias,
					Image = x.c.Image
				}).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<CategoryViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

		public async Task<List<CategoryViewModel>> GetAll()
        {
            var query = from c in _context.Categories
                        select new { c };

            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                idParent = x.c.idParent,
                Alias = x.c.Alias,
				Image = x.c.Image
			}).ToListAsync();
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            var query = from c in _context.Categories
                        where c.Id == id
                        select new { c };

            return await query.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                idParent = x.c.idParent,
                Alias = x.c.Alias,
				Image = x.c.Image
			}).FirstOrDefaultAsync();
        }


        // Get All CategortTree
		public async Task<List<CategoryTreeNode>> LoadCategoryTrees()
		{
			var getAllCategory = await _context.Categories.Where(c => c.isEnable).ToListAsync();
			var listCategoryClass = CategoryTreeHelper.BuildCategoryTree(getAllCategory);
			return listCategoryClass;
		}


        // Get ParentCategoryTree
		public async Task<CategoryTreeNodeParent> LoadCategoryTreeById(int categoryId)
		{
            try
            {
				var getAllCategory = await _context.Categories.Where(c => c.isEnable).ToListAsync();
				var categoryTree = CategoryTreeHelper.BuildCategoryTreeById(getAllCategory, categoryId);
				return categoryTree;
			}
            catch (Exception ex)
            {
                return null;
            }
		}

        // Get Child CategoryTree - one level
        public async Task<List<CategoryViewModel>> LoadChildCategoryById(int categoryId)
        {
            try
            {
                var categories = await _context.Categories
                                                .Where(c => c.isEnable && c.idParent == categoryId)
                                                .Select(x => new CategoryViewModel()
                                                {
                                                        Id = x.Id,
                                                        Alias = x.Alias,
                                                        idParent = x.idParent,
                                                        Name = x.Name,
                                                        Image = x.Image
												}).ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

		private async Task<string> SaveFile(IFormFile file)
		{
			var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
			await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
			return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
		}
	}
}