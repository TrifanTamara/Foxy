using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IVocabRelRepo : IGenericRepository<VocabularRelationship>
    {
        Task<IEnumerable<VocabularRelationship>> GetByMainId(Guid mainId);
        Task<IEnumerable<VocabularRelationship>> GetByContainedId(Guid containedId);
    }
}
