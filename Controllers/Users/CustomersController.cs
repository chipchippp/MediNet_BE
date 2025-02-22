﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediNet_BE.Data;
using MediNet_BE.Models.Users;
using MediNet_BE.Dto;
using MediNet_BE.Interfaces;
using MediNet_BE.Models;
using MediNet_BE.Dto.Users;
using MediNet_BE.Repositories;
using MediNet_BE.Services.Image;
using MediNet_BE.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MediNet_BE.Controllers.Users
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUserRepo<Customer, CustomerDto> _customerRepo;
        private readonly IFileService _fileService;

        public CustomersController(IUserRepo<Customer, CustomerDto> customerRepo, IFileService fileService)
        {
            _customerRepo = customerRepo;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customersDto = await _customerRepo.GetAllUserAsync();
            foreach (var customerDto in customersDto)
            {
                customerDto.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, customerDto.Image);
            }

            return Ok(customersDto);
        }

        [HttpGet]
        [Route("id")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var customerDto = await _customerRepo.GetUserByIdAsync(id);
			if (customerDto == null)
			{
				return NotFound();
			}
			customerDto.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, customerDto.Image);

            return Ok(customerDto);
        }

		[HttpGet]
		[Route("email")]
		public async Task<ActionResult<CustomerDto>> GetCustomerByEmail(string email)
		{
			var customerDto = await _customerRepo.GetUserByEmailAsync(email);
			if (customerDto == null)
			{
				return NotFound();
			}
			customerDto.ImageSrc = String.Format("{0}://{1}{2}/{3}", Request.Scheme, Request.Host, Request.PathBase, customerDto.Image);

            return Ok(customerDto);
		}


        /// <summary>
        ///  Create Customer
        /// </summary>
        /// <param name="customerCreate"></param>
        /// <remarks>
        /// "Username" : "John",
        /// "Email": " dùng email thật của mình và sửa mailSettings trong appsetings.json",
        /// "Password": "123456",
        /// "Address": "12A - Abc",
        /// "PhoneNumber": "0123456789",
        /// "": "",
        /// </remarks>
        /// <returns></returns>
        
        [Authorize]
        [RequiresClaim(IdentityData.RoleClaimName, "Admin")]
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer([FromForm] CustomerDto customerCreate)
        {
            if (customerCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (customerCreate.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(customerCreate.ImageFile, "images/users/customers/");
                if (fileResult.Item1 == 1)
                {
                    customerCreate.Image = fileResult.Item2;
				}
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }
            var newCustomer = await _customerRepo.AddUserAsync(customerCreate);
            return newCustomer == null ? NotFound() : Ok(newCustomer);
        }

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateCustomer([FromQuery] int id, [FromForm] CustomerDto updatedCustomer)
        {
            var customer = await _customerRepo.GetUserByIdAsync(id);
            if (customer == null)
                return NotFound();
            if (updatedCustomer == null)
                return BadRequest(ModelState);
            if (id != updatedCustomer.Id)
                return BadRequest();
            if (updatedCustomer.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(updatedCustomer.ImageFile, "images/users/customers/");
                if (fileResult.Item1 == 1)
                {
                    updatedCustomer.Image = fileResult.Item2;
                    await _fileService.DeleteImage(customer.Image);
                }
                else
                {
                    return NotFound("An error occurred while saving the image!");
                }
            }
            await _customerRepo.UpdateUserAsync(updatedCustomer);

            return Ok("Update Successfully!");
        }

		[Authorize]
		[RequiresClaim(IdentityData.RoleClaimName, "Admin")]
		[HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteCustomer([FromQuery] int id)
        {
            var customer = await _customerRepo.GetUserByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _customerRepo.DeleteUserAsync(id);
            await _fileService.DeleteImage(customer.Image);
            return Ok("Delete Successfully!");
        }
    }
}
