using Business;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
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

        public PopulateDb(IUsersRepository userRepo, IVocabularTempRepo vocabRepo,
            IFormularTempRepo formRepo, IQuestionTempRepo qRepo, IAnswerTempRepo aRepo)
        {
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            _formularRepo = formRepo;
            _questRepo = qRepo;
            _ansRepo = aRepo;

            Populate();
        }


        public async Task Populate()
        {
            if (_vocabRepo.GetAll().Result.Count() == 0)
            {
                await PopulateVocabular();
                await PopulateGrammarReading();
            }
        }

        public async Task PopulateVocabular()
        {
            await _userRepo.Clear();

            await _vocabRepo.ClearAllVocab();
            string dirpath = Directory.GetCurrentDirectory() + @"\PopulateDb\DBJsons\";

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
                foreach (VItem x in items)
                {
                    if (x.word_type == null) x.word_type = 0;
                    await _vocabRepo.Add(VocabularTemplate.Create
                        (x.name, x.meaning, x.reading, (VocabularType)x.type,
                        x.required_level, x.meaning_mnemonic, x.reading_mnemonic));
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
                foreach (VItem x in items)
                {
                    if (x.word_type == null) x.word_type = 0;
                    await _vocabRepo.Add(VocabularTemplate.Create
                        (x.name, x.meaning, x.reading, (VocabularType)x.type,
                        x.required_level, x.meaning_mnemonic, x.reading_mnemonic));
                    await _vocabRepo.AddRelations(x.name, (VocabularType)x.type, x.components.ToList());
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
                foreach (VItem x in items)
                {
                    if (x.word_type == null) x.word_type = 0;
                    await _vocabRepo.Add(VocabularTemplate.Create
                        (x.name, x.meaning, x.reading, (VocabularType)x.type,
                        x.required_level, x.meaning_mnemonic, x.reading_mnemonic, (WordParticularType)x.word_type));
                    await _vocabRepo.AddRelations(x.name, (VocabularType)x.type, x.components.ToList());
                }
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
                            List<VocabularTemplate> wordsForQuest = new List<VocabularTemplate>();

                            foreach (var ans in quest.Answers)
                            {
                                List<VocabularTemplate> wordList = new List<VocabularTemplate>();
                                foreach (var word in ans.Words)
                                {
                                    VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                                    if (vt != null)
                                    {
                                        wordList.Add(vt);
                                    }
                                }

                                AnswerTemplate newAns = AnswerTemplate.Create(ans.Text, ans.IsTrue, wordList);
                                await _ansRepo.Add(newAns);
                                ansList.Add(newAns);
                            }
                            foreach (var word in quest.Words)
                            {
                                VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                                if (vt != null)
                                {
                                    wordsForQuest.Add(vt);
                                }
                            }

                            QuestionTemplate newQuest = QuestionTemplate.Create(quest.Content, wordsForQuest, ansList);
                            await _questRepo.Add(newQuest);
                            questList.Add(newQuest);
                        }

                        List<VocabularTemplate> wordsForm = new List<VocabularTemplate>();
                        foreach (var word in item.Words)
                        {
                            VocabularTemplate vt = await _vocabRepo.GetByTypeAndName(VocabularType.Word, word);
                            if (vt != null)
                            {
                                wordsForm.Add(vt);
                            }
                        }

                        await _formularRepo.Add(FormularTemplate.Create(
                                item.PartialViewId, item.Topic, item.Description, (FormularType)item.Type, wordsForm, questList));
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
