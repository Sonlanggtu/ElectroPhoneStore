using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { set; get; }

        [MaxLength(500)]
        public string Alias { set; get; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { set; get; }
        public string Details { set; get; }

        // thumbnail path
        public string Thumbnail { get; set; }

        // product image path
        public string ProductImage { get; set; }

        public Category Category { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public int? ViewCount { set; get; }
        public string Tags { set; get; }
        public bool IsDnable { set; get; }
        public int OrderFeatured { set; get; } = 5;

        // 1. Laptop Gaming, Đồ Họa - Kỹ Thuật
        // 2.  Đồ Họa - Kỹ Thuật
        // 3. Học Tập - Văn Phòng
        // 4. Cao Cấp - Sang Trọng
        // 5. Không thuộc loại nào
        public int Purpose { set; get; } = 5;
		public string ShortDescription { set; get; }
		public decimal DiscountPercentage { set; get; }

	}
}