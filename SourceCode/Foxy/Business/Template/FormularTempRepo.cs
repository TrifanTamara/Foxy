using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Template
{
    public class FormularTempRepo :
        GenericRepo<FormTemplate>, IFormularTempRepo
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IQuestionTempRepo _questRepo;

        public FormularTempRepo(IDatabaseContext databaseContext, IQuestionTempRepo questRepo ) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _questRepo = questRepo;
        }

        public async Task<List<FormTemplate>> GetByType(FormType type, String name)
        {
            return await _databaseContext.FormularTemplates.Where(formular => formular.Type == type).ToListAsync();
        }

        public override async Task<IEnumerable<FormTemplate>> GetAll()
        {
            return await _databaseContext.FormularTemplates.Include(ft => ft.QuestionTemplates)
                .ThenInclude(at => at.AnswerTemplates).ToListAsync();
        }

        public override async Task<FormTemplate> FindById(Guid id)
        {
            return (await GetAll()).ToList().FirstOrDefault(ft => ft.FormTemplateId == id);
        }
    }
}
