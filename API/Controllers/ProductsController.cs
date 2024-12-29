using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _repo;
        private IMapper _mapper;
        private readonly StoreContext _context;

        public ProductsController(IProductRepository repo, IMapper mapper, StoreContext context)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
        }
        // [HttpPost]
        // public async Task<Product> CreateProductAsync(ProductCreateDto productCreateDto)
        // {
        //     if (productCreateDto == null)
        //     {
        //         throw new ArgumentNullException(nameof(productCreateDto), "Product data cannot be null.");
        //     }

        //     // Find the ProductType by name
        //     var productType = await _context.ProductTypes
        //         .FirstOrDefaultAsync(pt => pt.Name == productCreateDto.ProductType);

        //     if (productType == null)
        //     {
        //         throw new ArgumentException($"Invalid product type: {productCreateDto.ProductType}");
        //     }

        //     // Create the Product entity
        //     var product = new Product
        //     {
        //         Name = productCreateDto.Name,
        //         Description = productCreateDto.Description,
        //         Price = productCreateDto.Price,
        //         PictureUrl = productCreateDto.PictureUrl,
        //         ProductType = productType,

        //     };

        //     // Add the product to the database
        //     _context.Products.Add(product);
        //     await _context.SaveChangesAsync();

        //     // Return the created product
        //     return product;
        // }
        [HttpPost]
        public async Task<ProductResponseDto> CreateProductAsync([FromForm] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null)
            {
                throw new ArgumentNullException(nameof(productCreateDto), "Product data cannot be null.");
            }

            // Find the ProductType by name
            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(pt => pt.Name == productCreateDto.ProductType);

            if (productType == null)
            {
                throw new ArgumentException($"Invalid product type: {productCreateDto.ProductType}");
            }

            // Handle the image upload
            string imageUrl = null;
            if (productCreateDto.PictureUrl != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "products");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + productCreateDto.PictureUrl.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await productCreateDto.PictureUrl.CopyToAsync(fileStream);
                }

                imageUrl = $"/images/products/{uniqueFileName}";
            }

            // Create the Product entity
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                PictureUrl = imageUrl, // Set the image URL
                ProductType = productType,
            };

            // Add the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var pt = new ProductTypeDto
            {
                Name = productType.Name,
                Id = productType.Id,
            };
            var productResponse = new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                ProductType = pt.Name
            };

            return productResponse;

        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(string sort = null, int? typeId = null, string search = null)
        {
            var products = await _repo.GetProductsAsync();
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            switch (sort?.ToLower())
            {
                case "priceasc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "pricedesc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
            }
            if (typeId.HasValue)
            {
                products = products.Where(p => p.ProductTypeId == typeId.Value).ToList();
            }
            var result = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}