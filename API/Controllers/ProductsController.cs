using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _repo;
        private IMapper _mapper;

        public ProductsController(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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