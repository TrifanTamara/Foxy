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

        public VocabularTempRepository(IDatabaseContext databaseContext, IVocabRelRepository vocabRelRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _relationRepo = vocabRelRepo;
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
        
    }
}
