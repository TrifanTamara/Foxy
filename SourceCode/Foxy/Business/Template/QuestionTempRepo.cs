using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Template
{
    public class QuestionTempRepo :
        GenericRepo<QuestionTemplate>, IQuestionTempRepo
    {
        private readonly IDatabaseContext _databaseContext;
        private IAnswerTempRepo _answerRepo;

        public QuestionTempRepo(IDatabaseContext databaseContext, IAnswerTempRepo answerRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _answerRepo = answerRepo;

        }

        public async Task ClearAllQuestions()
        {
            await _answerRepo.Clear();
            await Clear();
        }

    }
}
