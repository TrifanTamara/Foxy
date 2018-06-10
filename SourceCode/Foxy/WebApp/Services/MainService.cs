using Business.Wrappers;
using Data.Domain.Entities;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DTOs;
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
            for (int i = 0; i < rightAnswers.Count(); ++i)
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

        public async Task<MenuModel> GetMenuModel(string email)
        {
            User user = await _userRepo.GetByEmail(email);
            MenuModel model = new MenuModel();
            model.Username = user.Username;
            model.Level = user.Level;
            model.LessonNumber = (await _vocabRepo.GetVocabLesson(user.UserId)).Count();
            model.ReviewNumber = (await _vocabRepo.GetVocabForReview(user.UserId)).Count();

            return model;
        }

        public void UpdateItemMeaningNote(VocabularItem item, string note)
        {
            item.Update(note, item.ReadingNote, item.Favorite, item.UserSynonyms);
            _vocabRepo.Edit(item);
        }

        public async Task StartReviewSession (Guid userId, bool forLesson, List<VocabularWrapper> items = null)
        {
            ReviewModel model = new ReviewModel();
            if (forLesson)
                model.Reviewitems = new List<VocabularWrapper>(items);
            else
                model.Reviewitems = await _vocabRepo.GetVocabForReview(userId);
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

        public VocabularWrapper GetCurrentItem(Guid userId)
        {
            ReviewModel model = currentReviewSeesion[userId];
            if (model.Reviewitems.Count == 0)
                return null;
            
            return model.Reviewitems[0];
        }

        public async Task<AnswerStatusModel> UserAnswered(Guid userId, VocabularAnswerDto answer)
        {
            ReviewModel model = currentReviewSeesion[userId];
            if (model != null)
            {
                if (model.Reviewitems.Count() == 0) { return null; }
                else
                {
                    VocabularWrapper item = model.Reviewitems[0];
                    AnswerStatusModel result = new AnswerStatusModel();

                    result.Meaning = CheckMeaningAns(answer.Meaning, item.MeaningsList);

                    List<string> readingList = item.OnyomyIsMain ? item.OnyomiReading : item.KunyoumiReading;

                    if (item.Template.Type != Data.Domain.Entities.TemplateItems.VocabularType.Radical)
                        result.Reading = CheckReadingAns(answer.Reading, readingList);
                    else result.Reading = true;

                    result.Final = result.Meaning && result.Reading;

                    await _vocabRepo.AddAnswer(item.Item, result.Final);
                    await _vocabRepo.IsUserReadyForNextLevel(item.Item.UserId);

                    model.Reviewitems[0].Item = await _vocabRepo.FindById(item.Item.VocabularItemId);
                    result.LevelName = _vocabRepo.GrandLvlNameFromMini(model.Reviewitems[0].Item.CurrentMiniLevel);

                    if (result.Final == true)
                        model.Reviewitems.RemoveAt(0);


                    return result;
                }
            }

            return null;
        }
    }
}

