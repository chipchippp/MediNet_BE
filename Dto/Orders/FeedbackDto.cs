﻿using System.ComponentModel.DataAnnotations;
using MediNet_BE.Dto.Users;

namespace MediNet_BE.Dto.Orders
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        [Required]
        public int Vote { get; set; }
        public string ImagesFeedback { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public IFormFile[]? ImagesFeedbackFile { get; set; }
		public List<string> ImagesSrc { get; set; } = [];
        public CustomerDto? Customer { get; set; }
        public ProductDto? Product { get; set; }
    }
}
