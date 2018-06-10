using Business.Wrappers;
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

        public async Task<VocabularWrapper> GetWrappedItem(Guid userId, string name, VocabularType type)
        {
            VocabularTemplate vTemp = await _tempVocabRepo.GetByTypeAndName(type, name);
            if (vTemp != null)
            {
                VocabularItem item = await GetVocabByTemplateAndUser(vTemp.VocabularTemplateId, userId);
                return await WrapSingleVocabular(item);
            }

            return null;
        }

        public async Task<IEnumerable<VocabularItem>> GetVocabByUser(Guid userId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.UserId.Equals(userId)).ToListAsync();
        }

        //??????????
        public async Task<VocabularItem> GetVocabByTemplate(Guid templateId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.VocabularTemplateId.Equals(templateId)).FirstOrDefaultAsync();
        }

        public async Task<VocabularItem> GetVocabByTemplateAndUser(Guid templateId, Guid userId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.VocabularTemplateId.Equals(templateId) && x.UserId.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task<VocabularWrapper> WrapSingleVocabular(VocabularItem item)
        {
            VocabularTemplate vTemp = await _tempVocabRepo.FindById(item.VocabularTemplateId);

            List<VocabularRelationship> pairs = (await _relVocabRepo.GetByMainId(vTemp.VocabularTemplateId)).ToList();
            List<VocabularTemplate> templatesComponents = new List<VocabularTemplate>();
            foreach (var pair in pairs)
            {
                templatesComponents.Add(await _tempVocabRepo.FindById(pair.ContainedItemId));
            }
            return (new VocabularWrapper(item, vTemp, templatesComponents, GrandLvlNameFromMini(item.CurrentMiniLevel)));
        }

        public async Task<List<VocabularWrapper>> WrapVocabular(List<VocabularItem> oldItems)
        {
            List<VocabularWrapper> result = new List<VocabularWrapper>();
            foreach (VocabularItem item in oldItems)
            {
                result.Add(await WrapSingleVocabular(item));
            }
            return result;
        }

        public async Task UnlockElements(VocabularItem item)
        {
            User user = await _userRepo.FindById(item.UserId);
            List<VocabularItem> vocabs = new List<VocabularItem>();
            VocabularTemplate vTemp = await _tempVocabRepo.FindById(item.VocabularTemplateId);
            List<VocabularRelationship> pairs = (await _relVocabRepo.GetByContainedId(vTemp.VocabularTemplateId)).ToList();
            foreach (VocabularRelationship x in pairs)
            {
                VocabularItem vItem = await GetVocabByTemplateAndUser(x.MainItemId, user.UserId);
                if (vItem != null)
                {
                    vItem.Update(vItem.LockedComponents - 1, vItem.CurrentMiniLevel);
                    if (vItem.LockedComponents == 0 && vTemp.RequiredLevel <= user.Level)
                        vItem.Update(DateTime.Now);
                    await Edit(vItem);
                }
            }
        }

        public async Task AddToLesson(VocabularItem vocab)
        {
            VocabularTemplate vTemp = await _tempVocabRepo.FindById(vocab.VocabularTemplateId);
            if (vTemp.Type == VocabularType.Kanji || vTemp.Type == VocabularType.Radical ||
                (vTemp.Type == VocabularType.Word && vocab.LockedComponents == 0))
            {
                vocab.Update(DateTime.Now);
                await Edit(vocab);
            }
        }

        public async Task RemoveFromLesson(VocabularItem vocab)
        {
            vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel + 1);
            await Edit(vocab);
            VocabularTemplate vTemp = await _tempVocabRepo.FindById(vocab.VocabularTemplateId);
            if (vTemp.Type != VocabularType.Word)
            {
                await UnlockElements(vocab);
            }
        }

        public async Task IsUserReadyForNextLevel(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            int passedRadicals = (await GetVocabLessonByTypes(userId, user.Level, VocabularType.Radical, InfoLessonType.Passed)).Count;
            int totalRadicals = (await GetAllVocabByItemType(user.UserId, VocabularType.Radical)).Count;

            int passedKanjis = (await GetVocabLessonByTypes(userId, user.Level, VocabularType.Kanji, InfoLessonType.Passed)).Count;
            int totalKanjis = (await GetAllVocabByItemType(user.UserId, VocabularType.Kanji)).Count;

            int radPercent = (int)(passedRadicals * 100 / (float)totalRadicals);
            int kanjiPercent = (int)(passedKanjis * 100 / (float)totalKanjis);

            if (radPercent >= 80 && kanjiPercent >= 80)
                await PassToNextLevel(user.Level + 1, userId);
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
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(v.VocabularTemplateId);
                if (vTemp.RequiredLevel == level)
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
                int nr = await _tempVocabRepo.GetComponentsById(v.VocabularTemplateId);
                await Add(VocabularItem.Create(user.UserId, v.VocabularTemplateId, nr));
            }
        }

        public async Task<List<VocabularWrapper>> GetVocabLesson(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> lessons = new List<VocabularItem>();
            foreach (VocabularItem x in allUsersVocab)
            {
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(x.VocabularTemplateId);
                if (vTemp.Type == VocabularType.Radical || vTemp.Type == VocabularType.Kanji)
                {
                    if (vTemp.RequiredLevel == user.Level && x.CurrentMiniLevel == 0)
                    {
                        lessons.Add(x);
                    }
                }
                else
                {
                    //word
                    if (vTemp.RequiredLevel <= user.Level && x.CurrentMiniLevel == 0 && DateTime.Compare(x.UnlockTime, DateTime.Now) <= 0)
                    {
                        lessons.Add(x);
                    }
                }
            }
            return (await WrapVocabular(lessons));
        }



        public bool MiniIsGrand(MiniLevels minilevel, GrandLevels grandLevel)
        {
            switch (grandLevel)
            {
                case GrandLevels.Lesson:
                    if ((int)minilevel == (int)GrandLevels.Lesson)
                        return true;
                    break;
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

        public string GrandLvlNameFromMini(MiniLevels minilevel)
        {
            if (MiniIsGrand(minilevel, GrandLevels.Lesson))
                return "Lesson";
            if (MiniIsGrand(minilevel, GrandLevels.Seed))
                return "Seed";
            if (MiniIsGrand(minilevel, GrandLevels.Leaf))
                return "Leaf";
            if (MiniIsGrand(minilevel, GrandLevels.Bloom))
                return "Bloom";
            if (MiniIsGrand(minilevel, GrandLevels.Flourished))
                return "Flourished";
            return "Nothing";
        }

        public async Task<List<VocabularWrapper>> GetVocabItemsByStages(Guid userId, int level, VocabularType type, GrandLevels levelItem)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> lessons = new List<VocabularItem>();
            foreach (VocabularItem item in allUsersVocab)
            {
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(item.VocabularTemplateId);
                if (vTemp.Type != type) continue;

                if (vTemp.Type == VocabularType.Radical || vTemp.Type == VocabularType.Kanji)
                {
                    if (vTemp.RequiredLevel == level && MiniIsGrand(item.CurrentMiniLevel, levelItem))
                    {
                        lessons.Add(item);
                    }
                } else
                {
                    //word
                    if (MiniIsGrand(item.CurrentMiniLevel, levelItem))
                    {
                        lessons.Add(item);
                    }
                }
            }
            return (await WrapVocabular(lessons));
        }

        public async Task<List<VocabularWrapper>> GetAllVocabByItemType(Guid userId, VocabularType type)
        {
            List<VocabularWrapper> elements = await WrapVocabular((await GetVocabByUser(userId)).ToList());
            List<VocabularWrapper> elementsType = new List<VocabularWrapper>();
            foreach (var x in elements)
            {
                if (x.Template.Type == type) elementsType.Add(x);
            }
            return elementsType;
        }

            public async Task<List<VocabularWrapper>> GetVocabLessonByTypes(Guid userId, int level, VocabularType type, InfoLessonType requestedInfo)
        {
            List<VocabularWrapper> elements = await GetVocabLesson(userId);
            List<VocabularWrapper> elementsType = new List<VocabularWrapper>();
            foreach (var x in elements)
            {
                if (x.Template.Type == type) elementsType.Add(x);
            }
            if (requestedInfo == InfoLessonType.ActiveLesson)
                return elementsType;


            switch (requestedInfo)
            {
                case InfoLessonType.ViewedLesson:
                    return (await GetVocabItemsByStages(userId, level, type, GrandLevels.Seed));
                case InfoLessonType.Passed:
                    return (await GetVocabItemsByStages(userId, level, type, GrandLevels.Leaf));
            }
            
            return new List<VocabularWrapper>();
        }

        public bool ActiveForReview(VocabularItem item)
        {
            if ((int)item.CurrentMiniLevel == (int)GrandLevels.Flourished)
                return false;
            if (item.LastAnswer == false)
                return true;
            DateTime readyTime = item.LastTimeAnswered.AddMinutes(StaticInfo.minutesForLevel[(int)item.CurrentMiniLevel]);
            if (DateTime.Compare(readyTime, DateTime.Now) <= 0)
                return true;
            return false;
        }

        public async Task<List<VocabularWrapper>> GetVocabForReview(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> reviews = new List<VocabularItem>();
            foreach (VocabularItem x in allUsersVocab)
            {
                if (ActiveForReview(x))
                {
                    VocabularTemplate vTemp = await _tempVocabRepo.FindById(x.VocabularTemplateId);
                    reviews.Add(x);
                }
            }
            return (await WrapVocabular(reviews));
        }

        public async Task<List<VocabularItem>> GetItemsByGrandLevels(Guid userId, GrandLevels level)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            return allUsersVocab.Where(x => DateTime.Compare(x.UnlockTime, DateTime.Now) <= 0 &&
                (int)x.CurrentMiniLevel > (int)level - 1 && (int)x.CurrentMiniLevel <= (int)level).ToList();
        }

        public bool VocabInList(VocabularTemplate element, List<VocabularWrapper> elementList)
        {
            foreach (var vocab in elementList)
            {
                if (vocab.Name.Equals(element.Name) && vocab.Template.Type == element.Type)
                    return true;
            }

            return false;
        }

        public bool ValidForLesson(VocabularWrapper item, List<VocabularWrapper> totalList, List<VocabularWrapper> littleList)
        {
            for (int i = 0; i < item.Components.Count; ++i)
            {
                if (VocabInList(item.Components[i], totalList) && !VocabInList(item.Components[i], littleList))
                    return false;
            }

            return true;
        }

        public async Task<List<VocabularWrapper>> GetItemsForLesson(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularWrapper> finalList = new List<VocabularWrapper>();
            if (user != null)
            {
                List<VocabularWrapper> vocab = await GetVocabLesson(userId);
                int length = user.LessonSessionCount < vocab.Count ? user.LessonSessionCount : vocab.Count;
                //pay attention here, may be infinite
                for (int i = 0; finalList.Count < length; ++i)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(vocab.Count);
                    if (ValidForLesson(vocab[r], vocab, finalList))
                    {
                        finalList.Add(vocab[r]);
                        vocab.RemoveAt(r);
                    }
                }
                return finalList;
            }
            return finalList;
        }

    }
}
