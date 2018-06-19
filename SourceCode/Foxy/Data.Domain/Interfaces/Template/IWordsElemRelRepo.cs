using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.Template
{
    public interface IWordsElemRelRepo : IGenericRepository<WordsInText>
    {
        Task<IEnumerable<WordsInText>> GetByMainId(Guid mainId);
        Task<IEnumerable<WordsInText>> GetByWordId(Guid wordId);
    }
}
