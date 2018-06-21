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
            public string Name { get; set; }
            [JsonProperty(PropertyName = "meaning")]
            public string Meaning { get; set; }
            [JsonProperty(PropertyName = "reading")]
            public string Reading { get; set; }
            [JsonProperty(PropertyName = "type")]
            public byte Type { get; set; }
            [JsonProperty(PropertyName = "required_level")]
            public byte Required_level { get; set; }
            [JsonProperty(PropertyName = "meaning_mnemonic")]
            public string Meaning_mnemonic { get; set; }
            [JsonProperty(PropertyName = "reading_mnemonic")]
            public string Reading_mnemonic { get; set; }
            [JsonProperty(PropertyName = "word_type")]
            public byte? Word_type { get; set; }
            [JsonProperty(PropertyName = "components")]
            public IList<string> Components { get; set; }
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
        private readonly IAnswerTempRepo _ansRepo;
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
                throw new Exception(e.Message);
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
                            if (x.Word_type == null) x.Word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.Name, x.Meaning, x.Reading, (VocabularType)x.Type,
                                x.Required_level, x.Meaning_mnemonic, x.Reading_mnemonic));
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
                            if (x.Word_type == null) x.Word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.Name, x.Meaning, x.Reading, (VocabularType)x.Type,
                                x.Required_level, x.Meaning_mnemonic, x.Reading_mnemonic));
                            await _vocabRepo.AddRelations(x.Name, (VocabularType)x.Type, x.Components.ToList());
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
                            if (x.Word_type == null) x.Word_type = 0;
                            await _vocabRepo.Add(VocabularTemplate.Create
                                (x.Name, x.Meaning, x.Reading, (VocabularType)x.Type,
                                x.Required_level, x.Meaning_mnemonic, x.Reading_mnemonic, (WordType)x.Word_type));
                            await _vocabRepo.AddRelations(x.Name, (VocabularType)x.Type, x.Components.ToList());
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
                                        await _relationshipsRepo.Add(WordsInText.Create(newAns.AnswerTemplateId,
                                            vt.VocabularTemplateId, TextType.Answer));
                                    }
                                }
                            }

                            QuestionTemplate newQuest = QuestionTemplate.Create(quest.Content, ansList);
                            questList.Add(newQuest);

                            foreach (var word in quest.Words)
                            {
                                VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                                if (vt != null)
                                {
                                    await _relationshipsRepo.Add(WordsInText.Create(newQuest.QuestionTemplateId,
                                            vt.VocabularTemplateId, TextType.Question));
                                }
                            }
                        }

                        FormTemplate formular = FormTemplate.Create(item.PartialViewId, item.Topic,
                            item.Description, (FormType)item.Type, questList);
                        foreach (var word in item.Words)
                        {
                            VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                            if (vt != null)
                            {
                                await _relationshipsRepo.Add(WordsInText.Create(formular.FormTemplateId,
                                            vt.VocabularTemplateId, TextType.Form));
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
