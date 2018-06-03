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

        public static bool CheckReadingAns(string userAns, string rightAnsw)
        {
            return true;
        }

        public static bool CheckMeaningAns(string userAns, string rightAnsw)
        {
            return true;
            return (userAns.ToLower().Equals(rightAnsw.ToLower()));
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

        public void StartReviewSession(Guid userId, List<VocabularWrapper> items)
        {
            ReviewModel model = new ReviewModel();
            model.Reviewitems = new List<VocabularWrapper>(items);
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
                //change in vocabular wrapper so that I have a list of reading and meanings
                result.Meaning = CheckMeaningAns(ansMeaning, item.Template.Meaning);
                result.Reading = CheckReadingAns(ansMeaning, item.Template.Reading);
                result.Final = result.Meaning && result.Reading;

                //go with logic of back end. Answer right vs Answer wrong

                return result;
            }

            return null;
        }
    }
}

