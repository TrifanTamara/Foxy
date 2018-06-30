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
        GenericRepo<FormItem>, IFormularItemRepo
    {
        private readonly IDatabaseContext _databaseContext;

        private readonly IVocabularItemRepo _vocabRepo;
        private readonly IQuestionItemRepo _questRepo;

        private readonly IFormularTempRepo _formularTempRepo;
        private readonly IUserRepo _userRepo;

        private readonly IWordsElemRelRepo _relationshipsRepo;

        public FormularItemRepo(IDatabaseContext databaseContext, IFormularTempRepo formRepo,
            IVocabularItemRepo vocabRepo, IUserRepo userRepo, IQuestionItemRepo questionRepo, IWordsElemRelRepo rel) : base(databaseContext)
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
            
            FormItem fi = await GetItemByTemplateAndUser(userId, formularTemplateId);
            FormTemplate ft = await _formularTempRepo.FindById(formularTemplateId);

            if(fi!=null && ft != null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                //List<QuestionWrapper> questionList = new List<QuestionWrapper>();
                List<VocabularWrapper> wordList = new List<VocabularWrapper>();

                //if (ft.QuestionTemplates != null)
                //{
                //    foreach (var question in ft.QuestionTemplates)
                //    {
                //        questionList.Add(await _questRepo.GetWrappedItem(userId, question.QuestionTemplateId));
                //    }
                //}
                
                List<WordsInText> relList = (await _relationshipsRepo.GetByMainId(ft.FormTemplateId)).ToList();
                foreach (var rel in relList)
                {
                    VocabularTemplate vt = await _vocabRepo.GetVocabTemplate(rel.WordId);
                    if (vt != null) wordList.Add(await _vocabRepo.GetWrappedItem(userId, vt.Name, vt.Type));
                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                return new FormularWrapper(fi, ft, wordList);
            }
            return null;
        }

        public async Task<List<FormularWrapper>> WrapFormularList(List<FormItem> formulars)
        {
            
            List<FormularWrapper> result = new List<FormularWrapper>();
            foreach(var f in formulars)
            {
                
                result.Add(await GetWrappedItem(f.UserId, f.FormularTemplateId));
                
            }
            
            return result;
        }

        public async Task<List<FormularWrapper>> WrapFormularList(List<FormTemplate> formulars, Guid userId)
        {
            List<FormularWrapper> result = new List<FormularWrapper>();
            foreach (var f in formulars)
            {
                result.Add(await GetWrappedItem(userId, f.FormTemplateId));

            }

            return result;
        }

        public async Task<FormItem> GetItemByTemplateAndUser(Guid userId, Guid templateId)
        {
            List<FormItem> items = (await GetAll()).ToList();
            return items.Where(f => f.FormularTemplateId == templateId && f.UserId==userId).FirstOrDefault();
        }

        public async Task AddItemsForUser(Guid userId)
        {
            await _questRepo.AddItemsForUser(userId);
            List<FormTemplate> templates = (await _formularTempRepo.GetAll()).ToList();

            foreach (var temp in templates)
            {
                await Add(FormItem.Create(userId, temp.FormTemplateId));
            }
        }

        public async Task<List<FormularWrapper>> GetAllFormularsByUser(Guid userId)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<FormItem> list = (await GetAll()).Where(f => f.UserId == userId).ToList();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;


            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            List<FormularWrapper> listW = await WrapFormularList(list);
            watch2.Stop();
            var elapsedMs2 = watch2.ElapsedMilliseconds;


            return listW;
        }

        public async Task<List<FormularWrapper>> GetAllFormByUserAndType(Guid userId, FormType type)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<FormTemplate> templates = (await _formularTempRepo.GetAll()).Where(x => x.Type == type).ToList();
            List<FormularWrapper> list = (await WrapFormularList(templates, userId));
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return list;
        }

        public async Task<List<FormTemplate>> GetFavoriteFormsType(Guid userId, FormType type)
        {
            List<FormItem> items = (await GetAll()).Where(x => userId==x.UserId && x.Favorite).ToList();
            List<FormTemplate> result = new List<FormTemplate>();

            foreach(var item in items)
            {
                FormTemplate temp = await _formularTempRepo.FindById(item.FormularTemplateId);
                if(temp.Type == type) result.Add(temp);
            }
            result = result.OrderBy(x => x.PartialViewId).ToList();

            return result;
        }

            public async Task<FormularWrapper> GetByUserAndPvId(Guid userId, int pvId, FormType type)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            FormTemplate temp = (await _formularTempRepo.GetAll()).Where(x => x.PartialViewId == pvId && type == x.Type).FirstOrDefault();
            FormularWrapper result = await GetWrappedItem(userId, temp.FormTemplateId);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return result;
        }

        public async Task<List<QuestionWrapper>> GetQuestionsByUserAndPvId(Guid userId, int pvId, FormType type)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            FormTemplate ft = (await _formularTempRepo.GetAll()).Where(x => x.PartialViewId == pvId && x.Type == type).FirstOrDefault();
            FormItem fi = await GetItemByTemplateAndUser(userId, ft.FormTemplateId);
            List<QuestionWrapper> questionList = new List<QuestionWrapper>();
            if (fi != null && ft != null)
            {
                if (ft.QuestionTemplates != null)
                {
                    foreach (var question in ft.QuestionTemplates)
                    {
                        questionList.Add(await _questRepo.GetWrappedItem(userId, question.QuestionTemplateId));
                    }
                }
                
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return questionList;
        }

        public async Task<List<FormTemplate>> GetAllFormTempByType(FormType type)
        {
            List<FormTemplate> templates = (await _formularTempRepo.GetAll()).Where(x => x.Type == type).ToList();
            
            return templates;
        }
    }
}
