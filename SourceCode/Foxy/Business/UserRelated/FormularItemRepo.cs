using Business.Wrappers;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.Template;
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

        private readonly IWordsElemRelRepo _relationshipsRepo;

        public FormularItemRepo(IDatabaseContext databaseContext, IFormularTempRepo formRepo,
            IVocabularItemRepo vocabRepo, IUsersRepository userRepo, IQuestionItemRepo questionRepo, IWordsElemRelRepo rel) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            _questRepo = questionRepo;
            _formularTempRepo = formRepo;
            _relationshipsRepo = rel;
        }

        public async Task<FormularWrapper> GetWrappedItem(Guid userId, Guid formularTemplateId)
        {
            FormularItem fi = await GetItemByTemplate(formularTemplateId);
            FormularTemplate ft = await _formularTempRepo.FindById(formularTemplateId);

            if(fi!=null && ft != null)
            {
                List<QuestionWrapper> questionList = new List<QuestionWrapper>();
                List<VocabularWrapper> wordList = new List<VocabularWrapper>();

                if (ft.QuestionTemplates != null)
                {
                    foreach (var question in ft.QuestionTemplates)
                    {
                        questionList.Add(await _questRepo.GetWrappedItem(userId, question.QuestionTemplateId));
                    }
                }

                List<WordFormQuestAnsRel> relList = (await _relationshipsRepo.GetByMainId(ft.FormularTemplateId)).ToList();
                foreach (var rel in relList)
                {
                    VocabularTemplate vt = await _vocabRepo.GetVocabTemplate(rel.WordId);
                    if (vt != null) wordList.Add(await _vocabRepo.GetWrappedItem(userId, vt.Name, vt.Type));
                }

                return new FormularWrapper(fi, ft, questionList, wordList);
            }
            return null;
        }

        public async Task<List<FormularWrapper>> WrapFormularList(List<FormularItem> formulars)
        {
            List<FormularWrapper> result = new List<FormularWrapper>();
            foreach(var f in formulars)
            {
                result.Add(await GetWrappedItem(f.UserId, f.FormularId));
            }
            return result;
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
                await Add(FormularItem.Create(userId, temp.FormularTemplateId));
            }
        }

        public async Task<List<FormularWrapper>> GetAllFormularsByUser(Guid userId)
        {
            return await WrapFormularList((await GetAll()).Where(f => f.UserId == userId).ToList());
        }

        public async Task<List<FormularWrapper>> GetAllFormByUserAndType(Guid userId, FormularType type)
        {
            return (await GetAllFormularsByUser(userId)).Where(f => f.Template.Type == type).ToList();
        }
    }
}
