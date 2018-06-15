using Business;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Domain.Interfaces.Template;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.PopulateDb
{
    public class PopulateDb
    {
        public class VItem
        {
            [JsonProperty(PropertyName = "name")]
            public string name { get; set; }
            [JsonProperty(PropertyName = "meaning")]
            public string meaning { get; set; }
            [JsonProperty(PropertyName = "reading")]
            public string reading { get; set; }
            [JsonProperty(PropertyName = "type")]
            public byte type { get; set; }
            [JsonProperty(PropertyName = "required_level")]
            public byte required_level { get; set; }
            [JsonProperty(PropertyName = "meaning_mnemonic")]
            public string meaning_mnemonic { get; set; }
            [JsonProperty(PropertyName = "reading_mnemonic")]
            public string reading_mnemonic { get; set; }
            [JsonProperty(PropertyName = "word_type")]
            public byte? word_type { get; set; }
            [JsonProperty(PropertyName = "components")]
            public IList<string> components { get; set; }
        }

        public class FormItem
        {
            [JsonProperty(PropertyName = "Topic")]
            public string Topic { get; set; }
            [JsonProperty(PropertyName = "PartialViewId")]
            public int PartialViewId { get; set; }
            [JsonProperty(PropertyName = "Description")]
            public string Description { get; set; }
            [JsonProperty(PropertyName = "Type")]
            public byte Type { get; set; }
            [JsonProperty(PropertyName = "Words")]
            public IList<string> Words { get; set; }
            [JsonProperty(PropertyName = "Questions")]
            public IList<QuestItem> Questions { get; set; }
        }

        public class QuestItem
        {
            [JsonProperty(PropertyName = "Content")]
            public string Content { get; set; }
            [JsonProperty(PropertyName = "Words")]
            public IList<string> Words { get; set; }
            [JsonProperty(PropertyName = "Answers")]
            public IList<AnsItem> Answers { get; set; }
        }

        public class AnsItem
        {
            [JsonProperty(PropertyName = "Text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "IsTrue")]
            public bool IsTrue { get; set; }
            [JsonProperty(PropertyName = "Words")]
            public IList<string> Words { get; set; }
        }

        private IVocabularTempRepo _vocabRepo;
        private IUsersRepository _userRepo;
        private IFormularTempRepo _formularRepo;
        private IQuestionTempRepo _questRepo;
        private IAnswerTempRepo _ansRepo;
        private readonly ICommonRepo _commonRepo;
        private IWordsElemRelRepo _relationshipsRepo;

        public PopulateDb(IUsersRepository userRepo, IVocabularTempRepo vocabRepo,
            IFormularTempRepo formRepo, IQuestionTempRepo qRepo, IAnswerTempRepo aRepo, 
            ICommonRepo commonRepo, IWordsElemRelRepo relationshipsRepo)
        {
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            _formularRepo = formRepo;
            _questRepo = qRepo;
            _ansRepo = aRepo;
            _commonRepo = commonRepo;
            _relationshipsRepo = relationshipsRepo;
        }


        public async Task Populate()
        {
            try
            {
                if (_vocabRepo.GetAll().Result.Count() == 0)
                {
                    await PopulateVocabular();
                    await PopulateGrammarReading();
                }
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task PopulateVocabular()
        {
            await _userRepo.Clear();

            await _vocabRepo.ClearAllVocab();
            string dirpath = Directory.GetCurrentDirectory() + @"\PopulateDb\DBJsons\";
            try
            {
                using (StreamReader r = new StreamReader(dirpath + "Radicals.json"))
                {
                    string json = r.ReadToEnd();
                    List<VItem> items = new List<VItem>();
                    try
                    {
                        items = JsonConvert.DeserializeObject<IEnumerable<VItem>>(json).ToList();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    try
                    {
                        foreach (VItem x in items)
                        {
                            if (x.word_type == null) x.word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.name, x.meaning, x.reading, (VocabularType)x.type,
                                x.required_level, x.meaning_mnemonic, x.reading_mnemonic));
                        }
                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.Message);
                    }
                }
                using (StreamReader r = new StreamReader(dirpath + "Kanjis.json"))
                {
                    string json = r.ReadToEnd();
                    List<VItem> items = new List<VItem>();
                    try
                    {
                        items = JsonConvert.DeserializeObject<IEnumerable<VItem>>(json).ToList();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    try
                    {
                        foreach (VItem x in items)
                        {
                            if (x.word_type == null) x.word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.name, x.meaning, x.reading, (VocabularType)x.type,
                                x.required_level, x.meaning_mnemonic, x.reading_mnemonic));
                            await _vocabRepo.AddRelations(x.name, (VocabularType)x.type, x.components.ToList());
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                using (StreamReader r = new StreamReader(dirpath + "Words.json"))
                {
                    string json = r.ReadToEnd();
                    List<VItem> items = new List<VItem>();
                    try
                    {
                        items = JsonConvert.DeserializeObject<IEnumerable<VItem>>(json).ToList();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    try
                    {
                        foreach (VItem x in items)
                        {
                            if (x.word_type == null) x.word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.name, x.meaning, x.reading, (VocabularType)x.type,
                                x.required_level, x.meaning_mnemonic, x.reading_mnemonic, (WordParticularType)x.word_type));
                            await _vocabRepo.AddRelations(x.name, (VocabularType)x.type, x.components.ToList());
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }

                await _vocabRepo.CalcTotalNumberLevel();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task PopulateGrammarReading()
        {
            await _formularRepo.Clear();
            await _questRepo.ClearAllQuestions();

            string dirpath = Directory.GetCurrentDirectory() + @"\PopulateDb\DBJsons\";
            using (StreamReader r = new StreamReader(dirpath + "Grammar.json"))
            {
                string json = r.ReadToEnd();
                List<FormItem> items = new List<FormItem>();
                try
                {
                    items = JsonConvert.DeserializeObject<IEnumerable<FormItem>>(json).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                try
                {
                    foreach (FormItem item in items)
                    {
                        List<QuestionTemplate> questList = new List<QuestionTemplate>();
                        foreach (var quest in item.Questions)
                        {
                            List<AnswerTemplate> ansList = new List<AnswerTemplate>();

                            foreach (var ans in quest.Answers)
                            {
                                AnswerTemplate newAns = AnswerTemplate.Create(ans.Text, ans.IsTrue);
                                ansList.Add(newAns);
                                foreach (var word in ans.Words)
                                {
                                    VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                                    if (vt != null)
                                    {
                                        await _relationshipsRepo.Add(WordFormQuestAnsRel.Create(newAns.AnswerTemplateId,
                                            vt.VocabularTemplateId, FQAElementType.Answer));
                                    }
                                }
                            }
                            foreach (var word in quest.Words)
                            {
                                QuestionTemplate newQuest = QuestionTemplate.Create(quest.Content, ansList);
                                questList.Add(newQuest);
                                VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                                if (vt != null)
                                {
                                    await _relationshipsRepo.Add(WordFormQuestAnsRel.Create(newQuest.QuestionTemplateId,
                                            vt.VocabularTemplateId, FQAElementType.Question));
                                }
                            }
                        }

                        FormularTemplate formular = FormularTemplate.Create(item.PartialViewId, item.Topic,
                            item.Description, (FormularType)item.Type, questList);
                        foreach (var word in item.Words)
                        {
                            VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                            if (vt != null)
                            {
                                await _relationshipsRepo.Add(WordFormQuestAnsRel.Create(formular.FormularTemplateId,
                                            vt.VocabularTemplateId, FQAElementType.Formular));
                            }
                        }
                        await _commonRepo.SaveFormular(formular);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
