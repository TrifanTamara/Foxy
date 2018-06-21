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
    public class VocabularTempRepo :
        GenericRepo<VocabularTemplate>, IVocabularTempRepo
    {
        private readonly IDatabaseContext _databaseContext;
        //private DbSet<VocabularTemplate> _entitiesVocab;
        private IVocabRelRepo _relationRepo;

        public VocabularTempRepo(IDatabaseContext databaseContext, IVocabRelRepo vocabRelRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _relationRepo = vocabRelRepo;
        }

        public async Task ClearAllVocab()
        {
            await _relationRepo.Clear();
            await Clear();
        }

        public async Task<int> GetComponentsById(Guid id)
        {
            List<VocabularRelationship> relations = (await _relationRepo.GetAll()).ToList();
            return (relations.Where(x => x.MainItemId.Equals(id))).Count();
        }

        public async Task<List<VocabularTemplate>> GetComponentsByLevel(int level)
        {
            List<VocabularTemplate> vocab = (await GetAll()).ToList();
            return (vocab.Where(x => x.RequiredLevel == level).ToList());
        }

        public async Task<VocabularTemplate> GetByTypeAndName(VocabularType type, String name)
        {
            return await _databaseContext.VocabularTemplates.FirstOrDefaultAsync(vocab => vocab.Type == type && vocab.Name.Equals(name, StringComparison.InvariantCulture));
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
                        await _relationRepo.Add(VocabularRelationship.Create(item.VocabularTemplateId, constructionVocab.VocabularTemplateId));
                    }
                }
            }
        }

        public async Task CalcTotalNumberLevel()
        {
            List<VocabularTemplate> allV = (await GetAll()).ToList();
            int maxLevel = 0;
            foreach(var v in allV)
            {
                maxLevel = maxLevel > v.RequiredLevel ? maxLevel : v.RequiredLevel;
            }
            StaticInfo.TotalLevelNumber = maxLevel;
        }

    }
}
