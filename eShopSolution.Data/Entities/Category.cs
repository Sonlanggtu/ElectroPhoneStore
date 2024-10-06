using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(500)]
        public string Alias { get; set; }
        public List<Product> Products { get; set; }

        public int idParent { set; get; }

        public bool isEnable { set; get; }

        [Column(TypeName = "varchar")]
        [MaxLength(500)]
        public string Image { get; set; }

    }
}
