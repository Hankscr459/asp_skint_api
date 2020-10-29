using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [AllowAnonymous]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(int id)
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [AllowAnonymous]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(int id)
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]Product product)
        {
            string webRootPath = _hostEnvironment.WebRootPath;

            // We need to retrieve all the files that were uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                // We will have a string file name on the file names for the images
                string fileName = Guid.NewGuid().ToString();

                // We need to navigate to the path of images and product
                var uploads = Path.Combine(webRootPath, @"images\products");
                var extenstion = Path.GetExtension(files[0].FileName);
                
                using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }
                product.PictureUrl = "images/products/" + fileName + extenstion;
            }
            _productsRepo.Add(product);
            if (await _productsRepo.SaveAll())
            {
                return Ok(product);
            }
            throw new Exception("Creating the Product failed on save");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm]Product product)
        {
            product.Id = id;
            string webRootPath = _hostEnvironment.WebRootPath;
             if (ModelState.IsValid) 
             {
                // We need to retrieve all the files that were uploaded
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    // We will have a string file name on the file names for the images
                    string fileName = Guid.NewGuid().ToString();

                    // We need to navigate to the path of images and product
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (product.PictureUrl != null)
                    {
                        // this is edit and we need to remove old image
                        var imagePath = Path.Combine(webRootPath, product.PictureUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    product.PictureUrl = "images/products/" + fileName + extenstion;
                    
                } 
                if (product.PictureUrl == null)
                {
                    var spec = new ProductsWithTypesAndBrandsSpecification(id);
                    Product objFormDb = await _productsRepo.GetEntityWithSpec(spec);
                    product.PictureUrl = objFormDb.PictureUrl;
                }

                _productsRepo.UpdateProduct(product);

                if (await _productsRepo.SaveAll())
                {
                    return Ok(product);
                }
                
            }
            throw new Exception("Fail to Update");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);
            _productsRepo.Delete(product);
            if (await _productsRepo.SaveAll())
            {
                return Ok();
            }
            throw new Exception("Fail to delete");
        }
    }
}