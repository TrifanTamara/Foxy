using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Data.Domain.Interfaces.Template;

namespace Business.Template
{
    public class WordsElemRelRepo :
        GenericRepo<WordsInText>, IWordsElemRelRepo
    {
        private readonly IDatabaseContext _databaseContext;
        
        public WordsElemRelRepo(IDatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<WordsInText>> GetByMainId(Guid mainId)
        {
            return await _databaseContext.WordFormQuestAnsRels.Where(x => x.MainElementId.Equals(mainId)).ToListAsync();
        }

        public async Task<IEnumerable<WordsInText>> GetByWordId(Guid containedId)
        {
            return await _databaseContext.WordFormQuestAnsRels.Where(x => x.WordId.Equals(containedId)).ToListAsync();
        }
    }
}
