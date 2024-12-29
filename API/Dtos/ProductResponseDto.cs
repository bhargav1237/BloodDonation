using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Dtos
{
    public class ProductResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductType { get; set; } // Name of the product type
    }

    public class ProductTypeDto
    {
        public string Name { get; set; }
        public int Id { get; set; }

    }
}