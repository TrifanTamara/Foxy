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

        private IVocabularTempRepository _vocabRepo;
        private IUsersRepository _userRepo;

        public PopulateDb(IUsersRepository userRepo, IVocabularTempRepository vocabRepo)
        {
            _vocabRepo = vocabRepo;
            _userRepo = userRepo;
            Populate();
        }


        public async Task Populate()
        {
            if (_vocabRepo.GetAll().Result.Count() == 0)
            {
                await PopulateVocabular();
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
    }
}
