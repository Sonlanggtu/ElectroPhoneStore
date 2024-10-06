using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }
        [Display(Name = "Alias danh mục")]
        public string Alias { get; set; }

		[Display(Name = "IdCategoryParent")]
		public int idParent { set; get; }

		[Display(Name = "Ảnh Icon")]
		public IFormFile Image { get; set; }
	}
}
