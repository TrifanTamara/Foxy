using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IQuestionTempRepo : IGenericRepository<QuestionTemplate>
    {
        //Task AddQuestionsForNewUser(Guid userId);
        Task ClearAllQuestions();
    }
}
