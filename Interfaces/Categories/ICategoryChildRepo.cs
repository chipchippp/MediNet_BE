﻿using MediNet_BE.Dto.Categories;
using MediNet_BE.Models.Categories;

namespace MediNet_BE.Interfaces.Categories
{
    public interface ICategoryChildRepo
    {
        public Task<List<CategoryChildDto>> GetAllCategoryChildAsync();
        public Task<CategoryChildDto> GetCategoryChildByIdAsync(int id);
        public Task<CategoryChild> AddCategoryChildAsync(CategoryChildDto categoryChildDto);
        public Task UpdateCategoryChildAsync(CategoryChildDto categoryChildDto);
        public Task DeleteCategoryChildAsync(int id);
    }
}
