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
    public class QuestionItemRepo :
        GenericRepo<QuestionItem>, IQuestionItemRepo
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IVocabularItemRepo _vocabRepo;

        private readonly IQuestionTempRepo _questionTempRepo;
        private readonly IUsersRepository _userRepo;
        
        private readonly IWordsElemRelRepo _relationshipsRepo;

        public QuestionItemRepo(IDatabaseContext databaseContext,
            IVocabularItemRepo vocabRepo, IUsersRepository userRepo, 
            IQuestionTempRepo questionRepo, IWordsElemRelRepo relationshipsRepo) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            _questionTempRepo = questionRepo;
            _relationshipsRepo = relationshipsRepo;
        }

        public async Task<QuestionWrapper> GetWrappedItem(Guid userId, Guid questionTemplateId)
        {
            
            QuestionItem qItem = await GetItemByTemplate(questionTemplateId);
            QuestionTemplate qTemp = await _questionTempRepo.FindById(questionTemplateId);
            if (qTemp != null && qItem!=null)
            {
                List<VocabularWrapper> vwList = new List<VocabularWrapper>();
                List<WordFormQuestAnsRel> relList = (await _relationshipsRepo.GetByMainId(qTemp.QuestionTemplateId)).ToList();
                foreach(var rel in relList)
                {
                    VocabularTemplate vt = await _vocabRepo.GetVocabTemplate(rel.WordId);
                    if(vt!=null) vwList.Add(await _vocabRepo.GetWrappedItem(userId, vt.Name, vt.Type));
                }

                foreach (var answer in qTemp.AnswerTemplates)
                {
                    relList = (await _relationshipsRepo.GetByMainId(answer.AnswerTemplateId)).ToList();
                    foreach (var rel in relList)
                    {
                        VocabularTemplate vt = await _vocabRepo.GetVocabTemplate(rel.WordId);
                        vwList.Add(await _vocabRepo.GetWrappedItem(userId, vt.Name, vt.Type));
                    }
                }

                return new QuestionWrapper(qItem, qTemp, vwList);
            }
            
            return null;
        }
        
        public async Task<QuestionItem> GetItemByTemplate(Guid templateId)
        {
            return _databaseContext.QuestionItems.Where(q => q.QuestionId == templateId).FirstOrDefault();
        }

        public async Task AddItemsForUser(Guid userId)
        {
            List<QuestionTemplate> templates = (await _questionTempRepo.GetAll()).ToList();

            foreach(var temp in templates)
            {
                await Add(QuestionItem.Create(userId, temp.QuestionTemplateId));
            }
        }
        
    }

}