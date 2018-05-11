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
        private readonly IUsersRepository _userRepo;
        //public List<VocabularItem> LessonList { get; private set; }
        //public List<VocabularItem> ReviewList { get; private set; }

        public VocabularItemRepository(IDatabaseContext databaseContext, IVocabularTempRepository tempVocabRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _tempVocabRepo = tempVocabRepo;
            //LessonList = new List<VocabularItem>();
            //ReviewList = new List<VocabularItem>();
        }

        //this should be called first 
        /*
        public async Task CreateLessonList()
        {
            LessonList.Clear();

        }

        //this should be called first
        public async Task CreateReviewList()
        {
            ReviewList.Clear();
        }
        */
        public async Task<IEnumerable<VocabularItem>> GetVocabByUser(Guid userId)
        {
            return await _databaseContext.VocabularItems.Where(x => x.UserId.Equals(userId)).ToListAsync();
        }

        public async Task AddToLesson(VocabularItem vocab)
        {
            vocab.Update(DateTime.Now);
            await Edit(vocab);
            //LessonList.Add(vocab);
        }
        

        public async Task RemoveFromLesson(VocabularItem vocab)
        {
            vocab.Update(vocab.LockedComponents, vocab.CurrentMiniLevel + 1);
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
        
    }
}
