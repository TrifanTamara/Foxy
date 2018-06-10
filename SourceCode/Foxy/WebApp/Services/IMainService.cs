using Business.Wrappers;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IMainService
    {
        Task<MenuModel> GetMenuModel(string email);
        void UpdateItemMeaningNote(VocabularItem item, string note);
        Task StartReviewSession(Guid userId, bool forLesson, List<VocabularWrapper> items = null);
        VocabularWrapper GetItemForReview(Guid userId);
        Task<AnswerStatusModel> UserAnswered(Guid userId, VocabularAnswerDto answer);
        VocabularWrapper GetCurrentItem(Guid userId);
    }
}
