using eShopSolution.Application.Common;
using eShopSolution.Application.Utilities;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.Utilities.Exceptions;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
//using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static eShopSolution.Utilities.Constants.SystemConstants;
using Review = eShopSolution.Data.Entities.Review;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        // dependency injection, truyền context vào để thao tác CRUD
        public ProductService(EShopDbContext context, IStorageService storageService, IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _storageService = storageService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            try
            {
                var product = new Data.Entities.Product()
                {
                    // id tự tăng
                    Name = request.Name,
                    CategoryId = request.CategoryId,
                    Price = request.Price,
                    Stock = request.Stock,
                    Description = request.Description,
                    Details = request.Details,
                    IsDnable = false,
                    Alias = request.Alias,
                    OrderFeatured = request.OrderFeatured,
                    Purpose = request.Purpose,
                    ShortDescription = request.ShortDescription,
                    DiscountPercentage = request.DiscountPercentage,
                };

                //Save thumbnail image
                if (request.ThumbnailImage != null)
                {
                    product.Thumbnail = await this.SaveFile(request.ThumbnailImage);
                }

                //Save product image
                if (request.ProductImage != null)
                {
                    product.ProductImage = await this.SaveFiles(request.ProductImage);
                }

                _context.Products.Add(product);

                //trả về số lượng bản ghi maybe
                await _context.SaveChangesAsync();
                return product.Id;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            try
            {
                var product = await _context.Products.FindAsync(request.Id);
                if (product == null) throw new EShopException($"Không thể tìm sản phẩm có ID: {request.Id} ");

                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.Description = request.Description;
                product.Details = request.Details;
                product.Stock = request.Stock;
                product.Price = request.Price;
                product.Alias = request.Alias;
                product.OrderFeatured = request.OrderFeatured;
                product.Purpose = request.Purpose;
                product.ShortDescription = request.ShortDescription;
                product.DiscountPercentage = request.DiscountPercentage;

                //Save thumbnail image
                string thumbnailImage = string.Empty;               
                if (request.ThumbnailImage != null)
                {
                    thumbnailImage = await this.SaveFile(request.ThumbnailImage);
                }
                if (!string.IsNullOrEmpty(request.ThumbnailImageSavedStr)) // images has been saved
                {
                    thumbnailImage = thumbnailImage + request.ThumbnailImageSavedStr;
                }
                product.Thumbnail = thumbnailImage;



                string productImages = string.Empty;
                string productImageUpload = string.Empty;
                //Save product image
                if (request.ProductImage != null)
                {
                    productImageUpload = await this.SaveFiles(request.ProductImage);
                    productImages = productImages + productImageUpload;
                }



                if (!string.IsNullOrEmpty(request.ProductImageSavedStr)) // images has been saved
                {
                    string[] arrProductImage = request.ProductImageSavedStr.Split(",");

                    if (arrProductImage.Length > 0 && arrProductImage.Where(x => x.Contains("/user-content/")).Any())
                    {
                        string productImageSavedStr = string.Join(",", arrProductImage);
                        productImages = string.IsNullOrEmpty(productImages) ? productImageSavedStr : $"{productImages},{productImageSavedStr}";
                    }

                }

                product.ProductImage = productImages;

                var res = await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new EShopException($"Không thể tìm sản phẩm có ID: {productId}");

            var images = _context.Products.Where(i => i.Id == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.Thumbnail);
                await _storageService.DeleteFileAsync(image.ProductImage);
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddReview(ProductDetailViewModel model)
        {
            Guid userGuid = new Guid(model.UserCommentId.ToString());
            Review review = new Review()
            {
                ProductId = model.ProductId,
                Comments = model.Review,
                Rating = model.Rating,
                PublishedDate = DateTime.Now,
                UserId = userGuid
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return model.ProductId;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryId equals c.Id
                        select new { p, c };
            //2. filter
            if (request.Purpose != 0)
                query = query.Where(x => x.p.Purpose == request.Purpose);

            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Name.Contains(request.Keyword));

            if (!string.IsNullOrEmpty(request.CategoryUrl))
            {
                //query = query.Where(x => x.c.Alias.Trim() == request.CategoryUrl.Trim());
                var productCategoryAll = _context.Categories.Where(x => x.isEnable).ToList();

				var productCategoryTrees = CategoryTreeHelper.BuildCategoryTree(null, request.CategoryUrl.Trim(), productCategoryAll);
                List<CategoryTreeNode> listproductCategoryFlat = new List<CategoryTreeNode>();
                BuildListCategoryTreebyId(productCategoryTrees, ref listproductCategoryFlat);

                if (listproductCategoryFlat.Any())
                {
                    var idCategories = listproductCategoryFlat.Select(x => x.Id).ToList();
					query = query.Where(x => idCategories.Contains(x.c.Id));
				}

				//int[] numbers = new []{ 1,2,4,6,78,3};
				//query = query.Where(x => numbers.Contains(x.c.Id));
				//query = query.Where(x => x.c.Id == numbers);
			}
                

            if (request.CategoryId != null && request.CategoryId != 0)
                query = query.Where(x => x.p.CategoryId == request.CategoryId);

			if (!string.IsNullOrEmpty(request.ConditionPrice))
			{
				switch (request.ConditionPrice)
				{
					case "<10tr":
						query = query.Where(x => x.p.Price < 10000000);
						break;
					case "10tr-15tr":
						query = query.Where(x => x.p.Price >= 10000000 && x.p.Price <= 15000000);
						break;
					case "15tr-20tr":
						query = query.Where(x => x.p.Price >= 15000000 && x.p.Price <= 20000000);
						break;
					case "20tr-25tr":
						query = query.Where(x => x.p.Price >= 20000000 && x.p.Price <= 25000000);
						break;
					case "25tr-30tr":
						query = query.Where(x => x.p.Price >= 25000000 && x.p.Price <= 30000000);
						break;
					case ">25tr":
						query = query.Where(x => x.p.Price > 25000000);
						break;

					case "30tr-35tr":
						query = query.Where(x => x.p.Price >= 30000000 && x.p.Price <= 35000000);
						break;
					case "35tr-40tr":
						query = query.Where(x => x.p.Price >= 35000000 && x.p.Price <= 40000000);
						break;
					case ">40tr":
						query = query.Where(x => x.p.Price < 40000000);
						break;
				}
			}

			if (!string.IsNullOrEmpty(request.SortOption))
            {
                switch (request.SortOption)
                {
                    case "name_a-z":
                        query = query.OrderBy(x => x.p.Name);
                        break;

                    case "price_low_to_hight":
                        query = query.OrderBy(x => x.p.Price);
                        break;

                    case "price_hight_to_low":
                        query = query.OrderByDescending(x => x.p.Price);
                        break;
				}
            }

            //3. Paging
            int totalRow = await query.CountAsync();

			var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    CategoryId = x.p.CategoryId,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ThumbnailImage = x.p.Thumbnail,
                    ProductImage = x.p.ProductImage,
                    Alias = x.p.Alias,
                    OrderFeatured = x.p.OrderFeatured,
                    Purpose = x.p.Purpose,
					LaptopInfo = !string.IsNullOrEmpty(x.p.ShortDescription) ? GetLaptopInfoFromHtml(x.p.ShortDescription) : null,
                    DiscountPercentage = x.p.DiscountPercentage,
                    ShortDescription = x.p.ShortDescription,
                    DiscountPrice = x.p.DiscountPercentage != 0 ? (x.p.DiscountPercentage / 100) * x.p.Price : 0,
                    PriceAfterDiscount = x.p.DiscountPercentage != 0 ? Convert.ToInt32(x.p.Price - ((x.p.DiscountPercentage / 100) * x.p.Price)) : Convert.ToInt32(x.p.Price)
				}).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        private async Task<string> SaveFiles(List<IFormFile> files)
        {
            string pathFiles = string.Empty;
            string basePath = $"/{USER_CONTENT_FOLDER_NAME}";
            foreach (var file in files)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
                pathFiles = pathFiles + $"{basePath}/{fileName},";
            }
            if (!string.IsNullOrEmpty(pathFiles)) pathFiles = pathFiles.Remove(pathFiles.Length - 1);
            return pathFiles;
        }


        public async Task<ProductViewModel> GetById(int productId)
        {
            // Lấy danh mục của sản phẩm
            var categories = await (from c in _context.Categories
                                    join p in _context.Products on c.Id equals p.CategoryId
                                    select p.Name).ToListAsync();

            var product = await _context.Products.FindAsync(productId);

            // Lấy danh sách review
            var reviews = _context.Reviews.Where(x => x.ProductId.Equals(productId)).ToList();

            // Lấy danh sách star rating
            //var ratings = _context.Reviews.Where(d => d.ProductId.Equals(productId)).ToList();

            var listOfReviews = new List<ReviewViewModel>();
            foreach (var review in reviews)
            {
                var user = await _userManager.FindByIdAsync(review.UserId.ToString());
                listOfReviews.Add(new ReviewViewModel()
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    UserName = user.Name,
                    ProductId = review.ProductId,
                    Rating = review.Rating,
                    Comments = review.Comments,
                    PublishedDate = review.PublishedDate
                });
            }

            var productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name != null ? product.Name : null,
                Alias = product.Alias,
                CategoryId = product.CategoryId != 0 ? product.CategoryId : 0,
                //Category = category,
                Description = product.Description != null ? product.Description : null,
                Details = product.Details != null ? product.Details : null,
                Price = product.Price,
                Stock = product.Stock,
                ThumbnailImage = product.Thumbnail != null ? product.Thumbnail : string.Empty, //: "no-image.jpg",
                ProductImage = product.ProductImage != null ? product.ProductImage : string.Empty, //: "no-image.jpg",
                Reviews = listOfReviews,
                OrderFeatured = product.OrderFeatured,
                Purpose = product.Purpose,
				LaptopInfo = !string.IsNullOrEmpty(product.ShortDescription) ? GetLaptopInfoFromHtml(product.ShortDescription) : null,
                ShortDescription = product.ShortDescription,
                DiscountPercentage = product.DiscountPercentage,
				DiscountPrice = product.DiscountPercentage != 0 ? (product.DiscountPercentage / 100) * product.Price : 0,
				PriceAfterDiscount = product.DiscountPercentage != 0 ? Convert.ToInt32(product.Price - ((product.DiscountPercentage / 100) * product.Price)) : Convert.ToInt32(product.Price)
			};
            return productViewModel;
        }

        // giảm số lượng sản phẩm trong kho khi khách hàng tăng sl sp
        public async Task<bool> DecreaseStock(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) throw new EShopException($"Cannot find product with id: {productId} ");
            product.Stock -= quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryId equals c.Id
                        select new { p };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.p.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    CategoryId = x.p.CategoryId,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ThumbnailImage = x.p.Thumbnail,
                    ProductImage = x.p.ProductImage,
                    Alias = x.p.Alias,
                    OrderFeatured = x.p.OrderFeatured,
                    Purpose = x.p.Purpose,
					LaptopInfo = !string.IsNullOrEmpty(x.p.ShortDescription) ? GetLaptopInfoFromHtml(x.p.ShortDescription) : null,
                    ShortDescription = x.p.ShortDescription,
                    DiscountPercentage = x.p.DiscountPercentage,
					DiscountPrice = x.p.DiscountPercentage != 0 ? (x.p.DiscountPercentage / 100) * x.p.Price : 0,
					PriceAfterDiscount = x.p.DiscountPercentage != 0 ? Convert.ToInt32(x.p.Price - ((x.p.DiscountPercentage / 100) * x.p.Price)) : Convert.ToInt32(x.p.Price)
				}).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<List<ProductViewModel>> GetFeaturedProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryId equals c.Id
                        where p.OrderFeatured == ProductConstants.PRIORITY_ONE
                        select new { p };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    CategoryId = x.p.CategoryId,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ThumbnailImage = x.p.Thumbnail,
                    ProductImage = x.p.ProductImage,
                    Alias = x.p.Alias,
                    Purpose = x.p.Purpose,
                    OrderFeatured = x.p.OrderFeatured,
                    LaptopInfo = !string.IsNullOrEmpty(x.p.ShortDescription) ?  GetLaptopInfoFromHtml(x.p.ShortDescription) : null,
                    ShortDescription = x.p.ShortDescription,
                    DiscountPercentage = x.p.DiscountPercentage,
					DiscountPrice = x.p.DiscountPercentage != 0 ? (x.p.DiscountPercentage / 100) * x.p.Price : 0,
					PriceAfterDiscount = x.p.DiscountPercentage != 0 ? Convert.ToInt32(x.p.Price - ((x.p.DiscountPercentage / 100) * x.p.Price)) : Convert.ToInt32(x.p.Price)
				}).ToListAsync();

            return data;
        }

        

        public async Task<List<ProductViewModel>> GetLatestProducts(int take)
        {
            //1. Select join
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryId equals c.Id
                        select new { p };

            var data = await query.OrderByDescending(x => x.p.DateCreated).Take(take)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.p.Name,
                    CategoryId = x.p.CategoryId,
                    DateCreated = x.p.DateCreated,
                    Description = x.p.Description,
                    Details = x.p.Details,
                    Price = x.p.Price,
                    Stock = x.p.Stock,
                    ThumbnailImage = x.p.Thumbnail,
                    ProductImage = x.p.ProductImage,
                    Alias = x.p.Alias,
                    OrderFeatured = x.p.OrderFeatured,
                    Purpose = x.p.Purpose,
					LaptopInfo = !string.IsNullOrEmpty(x.p.ShortDescription) ? GetLaptopInfoFromHtml(x.p.ShortDescription) : null,
                    ShortDescription = x.p.ShortDescription,
                    DiscountPercentage = x.p.DiscountPercentage,
					DiscountPrice = x.p.DiscountPercentage != 0 ? (x.p.DiscountPercentage / 100) * x.p.Price : 0,
					PriceAfterDiscount = x.p.DiscountPercentage != 0 ? Convert.ToInt32(x.p.Price - ((x.p.DiscountPercentage / 100) * x.p.Price)) : Convert.ToInt32(x.p.Price)
				}).ToListAsync();

            return data;
        }

		#region Support Func
		public static LaptopInfo GetLaptopInfoFromHtml(string html)
		{
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(html);

			var productsProperty = document.DocumentNode.SelectNodes("//tr/td/text()");
			if (productsProperty != null && productsProperty.Any())
			{
				var laptopInfo = new LaptopInfo();
				foreach (var itemProduct in productsProperty)
				{
					string textNoDiacritics = eShopSolution.Utilities.Constants.Helper.RemoveDiacritics(itemProduct.InnerText);
					if (!string.IsNullOrEmpty(textNoDiacritics))
					{
						if (textNoDiacritics.ToUpper().Contains("CPU:"))
							laptopInfo.CPU = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("RAM"))
							laptopInfo.RAM = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("O CUNG"))
							laptopInfo.Disk = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("CARD DO HOA"))
							laptopInfo.GPU = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("MAN HINH"))
							laptopInfo.Display = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("CONG KET NOI"))
							laptopInfo.Port = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("PIN"))
							laptopInfo.Pin = itemProduct.InnerText;
						else if (textNoDiacritics.ToUpper().Contains("TRONG LUONG"))
							laptopInfo.Weight = itemProduct.InnerText;
                        else if (textNoDiacritics.ToUpper().Contains("KICH THUOC"))
                            laptopInfo.Size = itemProduct.InnerText;
                    }
				}
                return laptopInfo;
            }
            else
            {
                return null;
            }
		}

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


		public static void BuildListCategoryTreebyId(CategoryTreeNode categoryTreeNode, ref List<CategoryTreeNode> listCategoryTreeNodes)
		{
			if (categoryTreeNode != null)
			{
				listCategoryTreeNodes.Add(
						new CategoryTreeNode()
						{
							Id = categoryTreeNode.Id,
							Name = categoryTreeNode.Name,
							Alias = categoryTreeNode.Alias
						});
				BuildListSubCategoryTreebyId(categoryTreeNode.Children, ref listCategoryTreeNodes);
			}
		}

		public static void BuildListSubCategoryTreebyId(List<CategoryTreeNode> categoryTreeNodes, ref List<CategoryTreeNode> listCategoryTreeNodes)
		{
			if (categoryTreeNodes != null && categoryTreeNodes.Any())
			{
                foreach (var categoryTreeItem in categoryTreeNodes)
                {
					listCategoryTreeNodes.Add(
						new CategoryTreeNode()
						{
							Id = categoryTreeItem.Id,
							Name = categoryTreeItem.Name,
							Alias = categoryTreeItem.Alias
						});

					BuildListSubCategoryTreebyId(categoryTreeItem.Children, ref listCategoryTreeNodes);
				}
			}
		}
		#endregion
	}
}