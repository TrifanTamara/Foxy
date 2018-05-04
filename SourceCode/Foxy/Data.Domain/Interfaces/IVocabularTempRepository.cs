using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IVocabularTempRepository : IGenericRepository<VocabularTemplate>
    {
        Task ClearAllVocab();
        Task<VocabularTemplate> GetByTypeAndName(VocabularType type, String name);
        Task AddRelations(String itemName, VocabularType itemType, List<String> constructionElements);
    }
}
