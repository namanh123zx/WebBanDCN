﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBH_Shopping.Data.Validation;

namespace WebBH_Shopping.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		

		public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên Sản phẩm")]
        public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Tên Sản phẩm")]

		public string Description { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập Giá Sản phẩm")]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName ="decimal(8,2)")]
		public decimal Price { get; set; }
		[Required,Range(1, int.MaxValue, ErrorMessage ="Chọn một thương hiệu")]
		public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
        public int CategoryId { get; set; }

		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image { get; set; } 
		[NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; }
       

        // Other properties...

        public ICollection<OrderDetails> OrderDetails { get; set; } // Navigation property

		internal static IQueryable<ProductModel> Where(Func<object, bool> value)
		{
			throw new NotImplementedException();
		}
	}
}
