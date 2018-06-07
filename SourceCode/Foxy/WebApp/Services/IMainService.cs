﻿using Business.Wrappers;
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
        MenuModel GetMenuModel(string email);
        void UpdateItemMeaningNote(VocabularItem item, string note);
        void StartReviewSession(Guid userId, bool forLesson, List<VocabularWrapper> items = null);
        VocabularWrapper GetItemForReview(Guid userId);
        AnswerStatusModel UserAnswered(Guid userId, VocabularAnswerDto answer);
        VocabularWrapper GetCurrentItem(Guid userId);
    }
}
