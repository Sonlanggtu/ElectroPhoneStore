﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.Data.Entities
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(1000)]
        [Column(TypeName = "varchar")]
        public string Alias { set; get; }

        [Required]
        public int CategoryID { set; get; }

        [MaxLength(3000)]
        public string Image { set; get; }

        [Column(TypeName = "ntext")]
        public string Description { set; get; }

        [Column(TypeName = "ntext")]
        public string Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public DateTime? CreatedDate { set; get; }
        public DateTime? UpdatedDate { set; get; }
        public int? ViewCount { set; get; }
        public int IsDisable { set; get; }

        [ForeignKey("CategoryID")]
        public virtual PostCategory PostCategory { set; get; }

        public virtual IEnumerable<PostTag> PostTags { set; get; }
    }
}
