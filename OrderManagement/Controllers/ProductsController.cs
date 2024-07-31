using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api.Dtos;
using OrderManagement.Api.Errors;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Repository.Data;
using Stripe;
using Product = OrderManagement.Core.Entities.Product;

namespace OrderManagement.Api.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepositories<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly OrderManagementDbContext _dbContext;

        public ProductsController(IGenericRepositories<Product>productRepo,IMapper mapper
            ,OrderManagementDbContext dbContext)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllAsync()
        {
            var Products=await _productRepo.GetAllAsync();
            if (Products is null) return NotFound(new ApiResponse(404));
            var MappedProducts= _mapper.Map<IReadOnlyList<ProductToReturnDto>>(Products);
            return Ok(MappedProducts);
        }
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ProductToReturnDto>> GetById(int id)
        {
            var Product = await _productRepo.GetByIdAsync(id);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProduct = _mapper.Map<ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductToSaveDto product)
        {
            //Check By Name if Product is Already Exist
            if(!ModelState.IsValid) return BadRequest(new ApiResponse(400));
            var Product = new Product() 
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
            await _productRepo.AddAsync(Product);
            //wrong
            await _dbContext.SaveChangesAsync();
            var MappedProduct = _mapper.Map<ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }
        [Authorize(Roles ="Admin")]
        [HttpPut]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct([FromForm]ProductToSaveDto model,int productId)
        {
            if (productId == 0) return BadRequest(new ApiResponse(400,"Enter Id"));

            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Data Is Wrong"));

            var Product = new Product()
            {
                Id = productId,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
            };
            try
            {
                _productRepo.Update(Product);
                //wrong
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var MappedProduct = _mapper.Map<ProductToReturnDto>(Product);
            return Ok(MappedProduct);
        }
    }
}
