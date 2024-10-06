using eShopSolution.ViewModels.Sales;
using eShopSolution.ViewModels.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Sales
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
		public string note { get; set; }
	}
}