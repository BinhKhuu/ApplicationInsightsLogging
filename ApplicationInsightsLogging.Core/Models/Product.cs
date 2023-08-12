using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsightsLogging.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Supplier is Required")]
        public string Supplier { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Price should be at least one dollar")]
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
