using Data.Domain.Entities;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public class MainService : IMainService
    {
        private readonly IUsersRepository _userRepo;
        IVocabularItemRepository _vocabRepo;

        public MainService(IUsersRepository repository, IVocabularItemRepository vocabRepo)
        {
            _userRepo = repository;
            _vocabRepo = vocabRepo;
        }

        public MenuModel GetMenuModel(string email)
        {
            User user = _userRepo.GetByEmail(email).Result; 
            MenuModel model = new MenuModel();
            model.Username = user.Username;
            model.Level = user.Level;
            model.LessonNumber = _vocabRepo.GetVocabLesson(user.UserId).Result.Count();
            model.ReviewNumber = _vocabRepo.GetVocabForReview(user.UserId).Result.Count();

            return model;
        }

        public void UpdateItemMeaningNote(VocabularItem item, string note)
        {
            item.Update(note, item.ReadingNote, item.Favorite);
            _vocabRepo.Edit(item);
        }
    }
}
