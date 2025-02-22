﻿using MediNet_BE.Dto.Orders;
using MediNet_BE.Models.Orders;

namespace MediNet_BE.Interfaces.Orders
{
    public interface IFeedbackRepo
    {
        public Task<List<FeedbackDto>> GetAllFeedbackAsync();
        public Task<FeedbackDto> GetFeedbackByIdAsync(int id);
        public Task<FeedbackDto> GetFeedbackByProductIdAsync(int productId);
        public Task<Feedback> AddFeedbackAsync(FeedbackDto feedbackDto);
        public Task UpdateFeedbackAsync(FeedbackDto feedbackDto);
        public Task DeleteFeedbackAsync(int id);
    }
}
