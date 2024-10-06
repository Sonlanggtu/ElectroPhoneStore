using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntegration
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CategoryApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateCategory(CategoryCreateRequest request)
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddressBackend]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

			var requestContent = new MultipartFormDataContent();
			requestContent.Add(new StringContent(request.Name.ToString()), "Name");
			requestContent.Add(new StringContent(request.Alias.ToString()), "Alias");
			requestContent.Add(new StringContent(request.idParent.ToString()), "idParent");

			if (request.Image != null)
			{
				byte[] data;
				using (var br = new BinaryReader(request.Image.OpenReadStream()))
				{
					data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
				}
				ByteArrayContent bytes = new ByteArrayContent(data);
				requestContent.Add(bytes, "Image", request.Image.FileName);
			}

			var response = await client.PostAsync($"/api/categories/", requestContent);
			return response.IsSuccessStatusCode;
		}


		public async Task<bool> UpdateCategory(CategoryUpdateRequest request)
		{
			var sessions = _httpContextAccessor
				.HttpContext
				.Session
				.GetString(SystemConstants.AppSettings.Token);

			var client = _httpClientFactory.CreateClient();
			client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddressBackend]);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


			var requestContent = new MultipartFormDataContent();
			requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ImageSavedStr) ? string.Empty : request.ImageSavedStr), "ImageSavedStr");
			requestContent.Add(new StringContent(request.Name.ToString()), "Name");
			requestContent.Add(new StringContent(request.Alias.ToString()), "Alias");
			requestContent.Add(new StringContent(request.Id.ToString()), "Id");
			requestContent.Add(new StringContent(request.IdParent.ToString()), "idParent");

			if (request.Image != null)
			{
				byte[] data;
				using (var br = new BinaryReader(request.Image.OpenReadStream()))
				{
					data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
				}
				ByteArrayContent bytes = new ByteArrayContent(data);
				requestContent.Add(bytes, "Image", request.Image.FileName);
			}

			var response = await client.PutAsync($"/api/categories/updateCategory", requestContent);
			return response.IsSuccessStatusCode;
		}


		//public async Task<bool> UpdateProduct(ProductUpdateRequest request)
		//{
		//	var sessions = _httpContextAccessor
		//		.HttpContext
		//		.Session
		//		.GetString(SystemConstants.AppSettings.Token);

		//	var client = _httpClientFactory.CreateClient();
		//	client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddressBackend]);
		//	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

		//	var requestContent = new MultipartFormDataContent();

		//	if (request.ThumbnailImage != null)
		//	{
		//		byte[] data;
		//		using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
		//		{
		//			data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
		//		}
		//		ByteArrayContent bytes = new ByteArrayContent(data);
		//		requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
		//	}

		//	if (request.ProductImage != null)
		//	{
		//		foreach (var productImgItem in request.ProductImage)
		//		{
		//			byte[] data;
		//			using (var br = new BinaryReader(productImgItem.OpenReadStream()))
		//			{
		//				data = br.ReadBytes((int)productImgItem.OpenReadStream().Length);
		//			}
		//			ByteArrayContent bytes = new ByteArrayContent(data);

		//			requestContent.Add(bytes, "productImage", productImgItem.FileName);
		//		}
		//	}

		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ThumbnailImageSavedStr) ? string.Empty : request.ThumbnailImageSavedStr), "ThumbnailImageSavedStr");
		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ProductImageSavedStr) ? string.Empty : request.ProductImageSavedStr), "ProductImageSavedStr");
		//	requestContent.Add(new StringContent(request.OrderFeatured.ToString()), "OrderFeatured");
		//	requestContent.Add(new StringContent(request.Purpose.ToString()), "Purpose");
		//	requestContent.Add(new StringContent(request.Alias.ToString()), "Alias");
		//	requestContent.Add(new StringContent(request.Price.ToString()), "price");
		//	requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
		//	requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? " " : request.Name.ToString()), "name");
		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? " " : request.Description.ToString()), "description");
		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.ShortDescription) ? " " : request.ShortDescription.ToString()), "ShortDescription");
		//	requestContent.Add(new StringContent(request.DiscountPercentage.ToString()), "DiscountPercentage");
		//	requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Details) ? " " : request.Details.ToString()), "details");

		//	var response = await client.PutAsync($"/api/products/" + request.Id, requestContent);
		//	return response.IsSuccessStatusCode;
		//}



		//public async Task<bool> UpdateCategory(CategoryUpdateRequest request)
		//      {
		//          var sessions = _httpContextAccessor
		//              .HttpContext
		//              .Session
		//              .GetString(SystemConstants.AppSettings.Token);

		//          var client = _httpClientFactory.CreateClient();
		//          client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddressBackend]);
		//          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

		//          var json = JsonConvert.SerializeObject(request);
		//          var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
		//          var response = await client.PutAsync($"/api/categories/updateCategory", httpContent);
		//          return response.IsSuccessStatusCode;
		//      }

		public async Task<bool> DeleteCategory(int id)
        {
            return await Delete($"/api/categories/" + id);
        }

        public async Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<CategoryViewModel>>(
               $"/api/categories/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               $"&keyword={request.Keyword}&sortOption={request.SortOption}");

            return data;
        }

        public async Task<List<CategoryViewModel>> GetAll()
        {
            return await GetListAsync<CategoryViewModel>("/api/categories");
        }

        public async Task<CategoryViewModel> GetById(int id)
        {
            return await GetAsync<CategoryViewModel>($"/api/categories/{id}");
        }

		public async Task<List<CategoryTreeNode>> LoadCategoryTrees()
		{
			return await GetAsync<List<CategoryTreeNode>>($"/api/categories/loadCategoryTrees");
		}
		public async Task<CategoryTreeNodeParent> LoadCategoryTreeById(int categoryId)
		{
			return await GetAsync<CategoryTreeNodeParent>($"/api/categories/loadCategoryTreeById/{categoryId}");
		}

		public async Task<List<CategoryViewModel>> LoadChildCategoryById(int categoryId)
		{
			return await GetAsync<List<CategoryViewModel>>($"/api/categories/loadChildCategoryById/{categoryId}");
		}
	}
}