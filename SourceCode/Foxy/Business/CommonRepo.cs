using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.Template;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CommonRepo : ICommonRepo
    {
        private readonly IDatabaseContext _databaseContext;
        
        public CommonRepo(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task SaveFormular(FormTemplate formular)
        {
            foreach(var quest in formular.QuestionTemplates)
            {
                foreach(var ans in quest.AnswerTemplates)
                {
                    await _databaseContext.AnswerTemplates.AddAsync(ans);
                }
                await _databaseContext.QuestionTemplates.AddAsync(quest);
            }
            await _databaseContext.FormularTemplates.AddAsync(formular);
            await ((DatabaseContext)_databaseContext).SaveChangesAsync();
        }
        
    }
}
