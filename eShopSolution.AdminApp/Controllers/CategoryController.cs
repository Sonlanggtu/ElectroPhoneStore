using Microsoft.AspNetCore.Mvc;
using eShopSolution.ViewModels.Catalog.Categories;
using System.Threading.Tasks;
using eShopSolution.ApiIntegration;
using eShopSolution.ViewModels.Catalog.Products;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace eShopSolution.AdminApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1000)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var categoryTrees = new List<CategoryTreeModel>();

            var categoryPaging = await _categoryApiClient.GetAllPaging(request);
            var categoryAll = await _categoryApiClient.GetAll();
            foreach (var category in categoryAll)
            {
				var categoryItem = new CategoryTreeModel();
				categoryItem.id = category.Id.ToString();
				categoryItem.text = category.Name;
				categoryItem.parent = category.idParent == 0 ? "#" : category.idParent.ToString();
				categoryTrees.Add(categoryItem);
			}

            ViewData["CategoryAll"] = categoryAll;
			ViewBag.CategoryTree = JsonConvert.SerializeObject(categoryTrees);

			if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(categoryPaging);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryAll = await _categoryApiClient.GetAll();
            ViewData["CategoryAll"] = categoryAll;
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
				ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
				return View(request);
            }

            var result = await _categoryApiClient.CreateCategory(request);
            if (result)
            {
                TempData["CreateCategorySuccessful"] = "Thêm mới danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm danh mục thất bại");
			ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
			return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryApiClient.GetById(id);

            var editVm = new CategoryUpdateRequest()
            {
                Id = id,
                Name = categories.Name,
                IdParent = categories.idParent,
                Alias = categories.Alias,
                ImageSavedStr = categories.Image,

            };
			ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
			return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
				ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
				return View(request);
            }

            var result = await _categoryApiClient.UpdateCategory(request);
            if (result)
            {
                TempData["UpdateCategorySuccessful"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật danh mục thất bại");
			ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
			return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new CategoryDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _categoryApiClient.DeleteCategory(request.Id);
            if (result)
            {
                TempData["DeleteCategorySuccessful"] = "Xóa danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Xóa danh mục không thành công");
			ViewData["CategoryAll"] = await _categoryApiClient.GetAll();
			return View(request);
        }
    }
}