﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Interfaces;
using MediNet_BE.Repositories;
using MediNet_BE.Services.Image;
using MediNet_BE.Models.Users;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;
using MediNet_BE.Interfaces.Categories;
using MediNet_BE.Interfaces.Orders;
using MediNet_BE.Models;
using Microsoft.IdentityModel.Tokens;
using MediNet_BE.Dto.Users;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MediNet_BE.Controllers.Orders
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly ICategoryChildRepo _categoryChildRepo;
        private readonly IClinicRepo _clinicRepo;
        private readonly IFileService _fileService;

        public ProductsController(IProductRepo productRepo, ICategoryChildRepo categoryChildRepo, IClinicRepo clinicRepo, IFileService fileService)
        {
            _productRepo = productRepo;
            _categoryChildRepo = categoryChildRepo;
            _clinicRepo = clinicRepo;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetCategories()
        {
            var prdsDto = await _productRepo.GetAllProductAsync();
			foreach (var prdDto in prdsDto)
			{
				prdDto.ImageSrc = String.Format("{0}://{1}{2}/{3}",Request.Scheme, Request.Host,Request.PathBase,prdDto.Image);
			}
			return Ok(prdsDto);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var productDto = await _productRepo.GetProductByIdAsync(id);
			if (productDto == null)
			{
				return NotFound();
			}
			productDto.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, productDto.Image);

			return Ok(productDto);
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="productCreate"></param>
        /// /// <remarks>
        /// ManufacturerDate
        /// 2024-04-13T08:18:59.6300000
        /// </remarks>
        /// <returns></returns>

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto productCreate)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(productCreate.CategoryChildId);
            var clinic = await _clinicRepo.GetClinicByIdAsync(productCreate.ClinicId);

            if (categoryChild == null)
                return NotFound("Category Child Not Found!");
            if (clinic == null)
                return NotFound("Clinic Not Found!");
            if (productCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productCreate.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(productCreate.ImageFile, "images/products/");
                if (fileResult.Item1 == 1)
                {
                    productCreate.Image = fileResult.Item2;
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            var newProduct = await _productRepo.AddProductAsync(productCreate);
            return newProduct == null ? NotFound() : Ok(newProduct);
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromForm] ProductCreateDto updatedProduct)
        {
            var categoryChild = await _categoryChildRepo.GetCategoryChildByIdAsync(updatedProduct.CategoryChildId);
            var clinic = await _clinicRepo.GetClinicByIdAsync(updatedProduct.ClinicId);
            var product = await _productRepo.GetProductByIdAsync(id);

            if (categoryChild == null)
                return NotFound("Category Child Not Found!");
            if (clinic == null)
                return NotFound("Clinic Not Found!");
            if (product == null)
                return NotFound();
            if (updatedProduct == null)
                return BadRequest(ModelState);
            if (id != updatedProduct.Id)
                return BadRequest();

            if (updatedProduct.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(updatedProduct.ImageFile, "images/products/");
                if (fileResult.Item1 == 1)
                {
                    updatedProduct.Image = fileResult.Item2;
                    await _fileService.DeleteImage(product.Image);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }

            await _productRepo.UpdateProductAsync(updatedProduct);

            return Ok("Update Successfully!");
        }

        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepo.DeleteProductAsync(id);
            await _fileService.DeleteImage(product.Image);
            return Ok("Delete Successfully!");
        }
    }
}
