using Business.Wrappers;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.UserRelated;
using Data.Domain.Wrappers;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.UserRelated
{
    public class FormularItemRepo :
        GenericRepo<FormularItem>, IFormularItemRepo
    {
        private readonly IDatabaseContext _databaseContext;

        private readonly IVocabularItemRepo _vocabRepo;
        private readonly IQuestionItemRepo _questRepo;

        private readonly IFormularTempRepo _formularTempRepo;
        private readonly IUsersRepository _userRepo;

        public FormularItemRepo(IDatabaseContext databaseContext, IFormularTempRepo formRepo,
            IVocabularItemRepo vocabRepo, IUsersRepository userRepo, IQuestionItemRepo questionRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            _questRepo = questionRepo;
            _formularTempRepo = formRepo;
        }

        public async Task<FormularWrapper> GetWrappedItem(Guid userId, Guid formularTemplateId)
        {
            FormularItem fi = await GetItemByTemplate(formularTemplateId);
            FormularTemplate ft = await _formularTempRepo.FindById(formularTemplateId);

            if(fi!=null && ft != null)
            {
                List<QuestionWrapper> questionList = new List<QuestionWrapper>();
                List<VocabularWrapper> wordList = new List<VocabularWrapper>();

                foreach(var question in ft.Questions)
                {
                    questionList.Add(await _questRepo.GetWrappedItem(userId, question.Id));
                }

                foreach(var word in ft.Words)
                {
                    wordList.Add(await _vocabRepo.GetWrappedItem(userId, word.Name, word.Type));
                }

                return new FormularWrapper(fi, ft, questionList, wordList);
            }
            return null;
        }

        public async Task<FormularItem> GetItemByTemplate(Guid templateId)
        {
            return _databaseContext.FormularItems.Where(f => f.FormularId == templateId).FirstOrDefault();
        }

        public async Task AddItemsForUser(Guid userId)
        {
            await _questRepo.AddItemsForUser(userId);
            List<FormularTemplate> templates = (await _formularTempRepo.GetAll()).ToList();

            foreach (var temp in templates)
            {
                await Add(FormularItem.Create(userId, temp.Id));
            }
        }

    }
}
