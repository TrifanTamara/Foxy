using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Template
{
    public class AnswerTempRepo :
        GenericRepo<AnswerTemplate>, IAnswerTempRepo
    {
        private readonly IDatabaseContext _databaseContext;

        public AnswerTempRepo(IDatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
    }
}
