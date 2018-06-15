using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.Template
{
    public interface IWordsElemRelRepo : IGenericRepository<WordFormQuestAnsRel>
    {
        Task<IEnumerable<WordFormQuestAnsRel>> GetByMainId(Guid mainId);
        Task<IEnumerable<WordFormQuestAnsRel>> GetByWordId(Guid wordId);
    }
}
