using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    

    public class VocabularItemRepository :
        GenericRepository<VocabularItem>, IVocabularItemRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IVocabularTempRepository _tempVocabRepo;
        private readonly IVocabRelRepository _relVocabRepo;
        private readonly IUsersRepository _userRepo;

        public VocabularItemRepository(IDatabaseContext databaseContext, IVocabularTempRepository tempVocabRepo, IUsersRepository userRepo, IVocabRelRepository relationRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _tempVocabRepo = tempVocabRepo;
            _userRepo = userRepo;
            _relVocabRepo = relationRepo;
        }


        public async Task<IEnumerable<VocabularItem>> GetVocabByUser(Guid userId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.User.Id.Equals(userId)).ToListAsync();
        }

        //??????????
        public async Task<VocabularItem> GetVocabByTemplate(Guid templateId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.Vocabular.Id.Equals(templateId)).FirstOrDefaultAsync();
        }

        public async Task<VocabularItem> GetVocabByTemplateAndUser(Guid templateId, Guid userId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.Vocabular.Id.Equals(templateId) && x.User.Id.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task UnlockElements(VocabularItem item)
        {
            User user = item.User;
            List<VocabularItem> vocabs = new List<VocabularItem>();
            List<VocabularRelationship> pairs = (await _relVocabRepo.GetByContainedId(item.Vocabular.Id)).ToList();
            foreach (VocabularRelationship x in pairs)
            {
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(x.MainItemId);
                VocabularItem vItem = await GetVocabByTemplateAndUser(vTemp.Id, user.Id);
                if (vItem != null)
                {
                    vItem.Update(vItem.LockedComponents - 1, vItem.CurrentMiniLevel);
                    if (vItem.LockedComponents == 0 && vTemp.RequiredLevel<=user.Level)
                        vItem.Update(DateTime.Now);
                    await Edit(vItem);
                }
            }
        }

        public async Task AddToLesson(VocabularItem vocab)
        {   if (vocab.Vocabular.Type == VocabularType.Kanji || vocab.Vocabular.Type == VocabularType.Radical || 
                (vocab.Vocabular.Type == VocabularType.Word && vocab.LockedComponents==0))
            {
                vocab.Update(DateTime.Now);
                await Edit(vocab);
            }
        }

        public async Task RemoveFromLesson(VocabularItem vocab)
        {
            vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel + 1);
            await Edit(vocab);
            if (vocab.Vocabular.Type != VocabularType.Word)
            {
                await UnlockElements(vocab);
            }
        }

        public async Task AddAnswer(VocabularItem vocab, bool answer)
        {
            if (answer)
            {
                vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel + 1);
                int strike = vocab.CurrentStrike + 1;
                int maxstrike = (strike > vocab.LongestStrike) ? strike : vocab.LongestStrike;
                vocab.Update(strike, maxstrike, vocab.RightAnswers + 1, vocab.WrongAnswers, true, DateTime.Now);
            }
            else
            {
                if ((int)vocab.CurrentMiniLevel > 1)
                {
                    vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel - 1);
                }
                vocab.Update(0, vocab.LongestStrike, vocab.RightAnswers, vocab.WrongAnswers + 1, false, DateTime.Now);
            }
            await Edit(vocab);
        }

        public async Task PassToNextLevel(int level, Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            user.Update(level);
            await _userRepo.Edit(user);

            List<VocabularItem> vocab = (await GetVocabByUser(userId)).ToList();
            foreach (VocabularItem v in vocab)
            {
                if(v.Vocabular.RequiredLevel == level)
                {
                    await AddToLesson(v);
                }
            }
        }
        
        public async Task AddVocabularForNewUser(Guid userId)
        {
            List<VocabularTemplate> vocabList = (await _tempVocabRepo.GetAll()).ToList();
            User user = await _userRepo.FindById(userId);
            foreach (VocabularTemplate v in vocabList)
            {
                int nr = await _tempVocabRepo.GetComponentsById(v.Id);
                await Add(VocabularItem.Create(user, v, nr));
            }
        }

        public async Task<List<VocabularItem>> GetVocabLesson(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> lessons = new List<VocabularItem>();
            foreach(VocabularItem x in allUsersVocab)
            {
                if(x.Vocabular.Type == VocabularType.Radical || x.Vocabular.Type == VocabularType.Kanji)
                {
                    if(x.Vocabular.RequiredLevel == user.Level && x.CurrentMiniLevel == 0)
                    {
                        lessons.Add(x);
                    }
                }
                else
                {
                    //word
                    if(x.Vocabular.RequiredLevel<=user.Level && x.CurrentMiniLevel == 0 && DateTime.Compare(x.UnlockTime,DateTime.Now)<=0)
                    {
                        lessons.Add(x);
                    }
                }
            }
            return lessons;
        }

        public bool MiniIsGrand(MiniLevels minilevel, GrandLevels grandLevel)
        {
            switch (grandLevel)
            {
                case GrandLevels.Seed:
                    if ((int)minilevel <= (int)GrandLevels.Seed && (int)minilevel > (int)GrandLevels.Lesson)
                        return true;
                    break;
                case GrandLevels.Leaf:
                    if ((int)minilevel <= (int)GrandLevels.Leaf && (int)minilevel > (int)GrandLevels.Seed)
                        return true;
                    break;
                case GrandLevels.Bloom:
                    return ((int)minilevel == (int)GrandLevels.Bloom);
                case GrandLevels.Flourished:
                    return ((int)minilevel == (int)GrandLevels.Flourished);
            }
            return false;
        }

        public async Task<List<VocabularItem>> GetVocabViewedLesson(Guid userId, int level, VocabularType type, GrandLevels levelItem)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> lessons = new List<VocabularItem>();
            foreach (VocabularItem x in allUsersVocab)
            {
                if (x.Vocabular.Type != type) continue;

                if (x.Vocabular.Type == VocabularType.Radical || x.Vocabular.Type == VocabularType.Kanji)
                {
                    if (x.Vocabular.RequiredLevel == level && MiniIsGrand(x.CurrentMiniLevel, levelItem))
                    {
                        lessons.Add(x);
                    }
                }
            }
            return lessons;
        }

        public async Task<List<VocabularItem>> GetVocabLessonByTypes(Guid userId, int level, VocabularType type, InfoLessonType requestedInfo)
        {
            List<VocabularItem> elements = await GetVocabLesson(userId);
            List<VocabularItem> elementsType = new List<VocabularItem>();
            foreach(var x in elements)
            {
                if (x.Vocabular.Type == type) elementsType.Add(x);
            }
            if (requestedInfo == InfoLessonType.ActiveLesson)
                return elementsType;

            List<VocabularItem> result = new List<VocabularItem>();
            foreach(var x in elementsType)
            {
                switch (requestedInfo)
                {
                    case InfoLessonType.ViewedLesson:
                        return (await GetVocabViewedLesson(userId, level, type, GrandLevels.Seed));
                    case InfoLessonType.Passed:
                        return (await GetVocabViewedLesson(userId, level, type, GrandLevels.Leaf));
                } 
            }

            //default ?
            return elements;
        }

        public bool ActiveForReview(VocabularItem item)
        {
            if ((int)item.CurrentMiniLevel == (int)GrandLevels.Flourished)
                return false;
            DateTime readyTime = item.LastTimeAnswered.AddMinutes(StaticInfo.minutesForLevel[(int)item.CurrentMiniLevel]);
            if (DateTime.Compare(readyTime, DateTime.Now) <= 0)
                return true;
            return false;
        }

        public async Task<List<VocabularItem>> GetVocabForReview(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> reviews = new List<VocabularItem>();
            foreach (VocabularItem x in allUsersVocab)
            {
                if (ActiveForReview(x))
                    reviews.Add(x);
            }
            return reviews;
        }

        public async Task<List<VocabularItem>> GetItemsByGrandLevels(Guid userId, GrandLevels level)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            return allUsersVocab.Where(x => DateTime.Compare(x.UnlockTime, DateTime.Now)<=0 && 
                (int)x.CurrentMiniLevel > (int)level-1 && (int)x.CurrentMiniLevel <= (int)level).ToList();
        }

    }
}
