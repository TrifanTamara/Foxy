using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Persistence;
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
       
        public VocabularItemRepository(IDatabaseContext databaseContext, IVocabularTempRepository tempVocabRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _tempVocabRepo = tempVocabRepo;
        }
        
        public async Task UpdateUnlockTime(Guid userId, )

        public async Task AddVocabularForNewUser(Guid userId)
        {
            List<VocabularTemplate> vocabList = (await _tempVocabRepo.GetAll()).ToList();
            foreach (VocabularTemplate v in vocabList)
            {
                await Add(VocabularItem.Create(userId, v.Id));
            }
        }

        
    }
}
