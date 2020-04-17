using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gunjanelectronics.Models
{
	public class AddcustomerModel
	{
		public int id { get; set; }
		public int? Customerid { get; set; }
		[Required]
		[RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Only Alphabets are allowed")]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
		public DateTime? Dob { get; set; }
		public int? countryid { get; set; }
		public string countryname { get; set; }
		public int? stateid { get; set; }
		public string statename { get; set; }
		public int? cityid { get; set; }
		public string cityname { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
		public string wouldyouliketoparticipateinSurvey { get; set; }
	}
}