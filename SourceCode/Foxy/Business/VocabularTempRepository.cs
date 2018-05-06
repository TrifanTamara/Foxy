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
    public class VocabularTempRepository :
        GenericRepository<VocabularTemplate>, IVocabularTempRepository
    {
        private readonly IDatabaseContext _databaseContext;
        //private DbSet<VocabularTemplate> _entitiesVocab;
        private IVocabRelRepository _relationRepo;
        private IVocabularItemRepository _vocabItemRepo;

        public VocabularTempRepository(IDatabaseContext databaseContext, IVocabRelRepository vocabRelRepo, IVocabularItemRepository vocabItemRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _relationRepo = vocabRelRepo;
            _vocabItemRepo = vocabItemRepo;
        }

        public async Task ClearAllVocab()
        {
            await _relationRepo.Clear();
            await Clear();
        }

        public async Task<VocabularTemplate> GetByTypeAndName(VocabularType type, String name)
        {
            return await _databaseContext.VocabularTemplates.FirstOrDefaultAsync(vocab => vocab.Type == type && vocab.Name.Equals(name));
        }

        public async Task AddRelations(String itemName, VocabularType itemType, List<String> constructionElements)
        {
            VocabularTemplate item = await GetByTypeAndName(itemType, itemName);
            if (null != item)
            {
                foreach (String elem in constructionElements)
                {
                    VocabularTemplate constructionVocab = await GetByTypeAndName(item.Type - 1, elem);
                    if (null != constructionVocab)
                    {
                        await _relationRepo.Add(VocabularRelationship.Create(item.Id, constructionVocab.Id));
                    }
                }
            }
        }

        public async Task AddVocabularForNewUser(Guid userId)
        {
            List<VocabularTemplate> vocabList = (await GetAll()).ToList();
            foreach(VocabularTemplate v in vocabList)
            {
                await _vocabItemRepo.Add(VocabularItem.Create(userId, v.Id));
            }
        }

    }
}
