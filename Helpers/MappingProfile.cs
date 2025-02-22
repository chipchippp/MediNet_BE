﻿using AutoMapper;
using MediNet_BE.Dto;
using MediNet_BE.Dto.Categories;
using MediNet_BE.Dto.Orders;
using MediNet_BE.Dto.Users;
using MediNet_BE.Models;
using MediNet_BE.Models.Categories;
using MediNet_BE.Models.Orders;
using MediNet_BE.Models.Users;

namespace MediNet_BE.Helpers
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
			CreateMap<Customer, CustomerDto>();
			CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, RegisterRequest>();
            CreateMap<RegisterRequest, Customer>();
            CreateMap<Customer, AuthenticateRequest>();
            CreateMap<AuthenticateRequest, Customer>();

            CreateMap<Admin, AdminDto>();
			CreateMap<AdminDto, Admin>();

			CreateMap<Category, CategoryDto>();
			CreateMap<CategoryDto, Category>();

			CreateMap<CategoryChild, CategoryChildDto>();
			CreateMap<CategoryChildDto, CategoryChild>();

			CreateMap<Clinic, ClinicDto>();
			CreateMap<ClinicDto, Clinic>();

			CreateMap<Order, OrderDto>();
			CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderCreateDto>();
            CreateMap<OrderCreateDto, Order>();

            CreateMap<OrderProduct, OrderProductDto>();
			CreateMap<OrderProductDto, OrderProduct>();
			CreateMap<OrderService, OrderServiceDto>();
			CreateMap<OrderServiceDto, OrderService>();

			CreateMap<Product, ProductDto>();
			CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductCreateDto>();
            CreateMap<ProductCreateDto, Product>();

            CreateMap<Service, ServiceDto>();
			CreateMap<ServiceDto, Service>();
            CreateMap<Service, ServiceCreateDto>();
            CreateMap<ServiceCreateDto, Service>();

            CreateMap<Feedback, FeedbackDto>();
			CreateMap<FeedbackDto, Feedback>();
		}
	}
}
