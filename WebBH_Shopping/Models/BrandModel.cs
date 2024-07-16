using System.ComponentModel.DataAnnotations;

namespace WebBH_Shopping.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập Tên Thương hiệu")]

		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô tả Tên Thương Hiệu")]

		public string Description { get; set; }
		public string Slug { get; set; }
		public int Status { get; set; }
	}
}
