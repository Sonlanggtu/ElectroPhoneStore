﻿using eShopSolution.ViewModels.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Catalog.Categories
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; } 
        public int idParent { set; get; }
		public string Image { set; get; }
	}
}
