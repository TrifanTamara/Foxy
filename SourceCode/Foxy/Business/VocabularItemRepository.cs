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
            return await _databaseContext.VocabularItems.Where(x => x.UserId.Equals(userId)).ToListAsync();
        }

        public async Task<VocabularItem> GetVocabByTemplate(Guid templateId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.VocabularId.Equals(templateId)).FirstOrDefaultAsync();
        }


        public async Task UnlockElements(VocabularItem item)
        {
            List<VocabularItem> vocabs = new List<VocabularItem>();
            List<VocabularRelationship> pairs = (await _relVocabRepo.GetByContainedId(item.VocabularId)).ToList();
            foreach (VocabularRelationship x in pairs)
            {
                VocabularItem vItem = await GetVocabByTemplate(x.MainItemId);
                if (vItem != null)
                {
                    vItem.Update(vItem.LockedComponents - 1, vItem.CurrentMiniLevel);
                    if (vItem.LockedComponents == 0) vItem.Update(DateTime.Now);
                    await Edit(vItem);
                }
            }
        }

        public async Task AddToLesson(VocabularItem vocab)
        {
            vocab.Update(DateTime.Now);
            await Edit(vocab);
        }
        
        public async Task RemoveFromLesson(VocabularItem vocab)
        {
            vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel + 1);
            await Edit(vocab);
            await UnlockElements(vocab);
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
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(v.VocabularId);
                if(vTemp.RequiredLevel == level)
                {
                    await AddToLesson(v);
                }
            }
        }
        
        public async Task AddVocabularForNewUser(Guid userId)
        {
            List<VocabularTemplate> vocabList = (await _tempVocabRepo.GetAll()).ToList();
            foreach (VocabularTemplate v in vocabList)
            {
                int nr = await _tempVocabRepo.GetComponentsById(v.Id);
                await Add(VocabularItem.Create(userId, v.Id, nr));
            }
        }

        public async Task<List<VocabularItem>> GetVocabLesson(Guid userId)
        {
            User user = await _userRepo.FindById(userId);
            List<VocabularItem> allUsersVocab = (await GetVocabByUser(userId)).ToList();
            List<VocabularItem> lessons = new List<VocabularItem>();
            foreach(VocabularItem x in allUsersVocab)
            {
                VocabularTemplate vTemp = await _tempVocabRepo.FindById(x.VocabularId);
                if(vTemp.Type == VocabularType.Radical || vTemp.Type == VocabularType.Kanji)
                {
                    if(vTemp.RequiredLevel == user.Level && x.CurrentMiniLevel == 0)
                    {
                        lessons.Add(x);
                    }
                }
                else
                {
                    //word
                    if(vTemp.RequiredLevel<=user.Level && x.CurrentMiniLevel == 0 && DateTime.Compare(x.UnlockTime,DateTime.Now)<=0)
                    {
                        lessons.Add(x);
                    }
                }
            }
            return lessons;
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
            return allUsersVocab.Where(x => (int)x.CurrentMiniLevel > (int)level-1 && (int)x.CurrentMiniLevel <= (int)level).ToList();
        }

    }
}
