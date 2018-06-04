using Business.Wrappers;
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
        private static Dictionary<Guid, ReviewModel> currentReviewSeesion = new Dictionary<Guid, ReviewModel>();

        public static bool CheckReadingAns(string userAns, List<string> rightAnswers)
        {
            for(int i=0; i<rightAnswers.Count(); ++i)
            {
                if (rightAnswers[i].ToLower().Equals(userAns.ToLower()))
                    return true;
            }
            return false;
        }

        public static bool CheckMeaningAns(string userAns, List<string> rightAnswers)
        {
            for (int i = 0; i < rightAnswers.Count(); ++i)
            {
                if (rightAnswers[i].ToLower().Equals(userAns.ToLower()))
                    return true;
            }
            return false;
        }

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
            item.Update(note, item.ReadingNote, item.Favorite, item.UserSynonyms);
            _vocabRepo.Edit(item);
        }

        public void StartReviewSession(Guid userId, bool forLesson, List<VocabularWrapper> items = null)
        {
            ReviewModel model = new ReviewModel();
            if (forLesson)
                model.Reviewitems = new List<VocabularWrapper>(items);
            else
                model.Reviewitems = _vocabRepo.GetVocabForReview(userId).Result;
            currentReviewSeesion[userId] = model;
        }
            
        public VocabularWrapper GetItemForReview(Guid userId)
        {
            ReviewModel model = currentReviewSeesion[userId];
            if (model.Reviewitems.Count == 0)
                return null;
            var rnd = new Random();
            model.Reviewitems = model.Reviewitems.OrderBy(item => rnd.Next()).ToList();
            return model.Reviewitems[0];
        }

        public AnswerStatusModel UserAnswered(Guid userId, string ansMeaning, string ansReading)
        {
            ReviewModel model = currentReviewSeesion[userId];
            if (model != null)
            {
                VocabularWrapper item = model.Reviewitems[0];
                AnswerStatusModel result = new AnswerStatusModel();
                
                result.Meaning = CheckMeaningAns(ansMeaning, item.MeaningsList);
                List<string> readingList = item.OnyomyIsMain ? item.OnyomiReading : item.KunyoumiReading;
                result.Reading = CheckReadingAns(ansMeaning, readingList);
                result.Final = result.Meaning && result.Reading;
                
                _vocabRepo.AddAnswer(item.Item, result.Final);
                model.Reviewitems[0].Item = _vocabRepo.FindById(item.Item.VocabularItemId).Result;
                result.LevelName = _vocabRepo.GrandLvlNameFromMini(model.Reviewitems[0].Item.CurrentMiniLevel);

                if (result.Final == true)
                    model.Reviewitems.RemoveAt(0);

                return result;
            }

            return null;
        }
    }
}

