﻿using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IVocabularTempRepo : IGenericRepository<VocabularTemplate>
    {
        Task ClearAllVocab();
        Task<VocabularTemplate> GetByTypeAndName(VocabularType type, String name);
        Task AddRelations(String itemName, VocabularType itemType, List<String> constructionElements);
        Task<int> GetComponentsById(Guid id);
        Task CalcTotalNumberLevel();
        Task<List<VocabularTemplate>> GetComponentsByLevel(int level);
    }
}
