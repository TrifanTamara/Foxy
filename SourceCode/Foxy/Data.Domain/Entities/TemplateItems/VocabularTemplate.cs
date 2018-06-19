using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;

namespace Data.Domain.Entities.TemplateItems
{
    public enum VocabularType : byte
    {
        Radical,
        Kanji,
        Word,
        All 
    }

    public enum WordType : byte
    {
        IAdj,
        NaAdj,
        Verb,
        Expression,
        Noun,
        Common,
        Numeral
    }

    public class VocabularTemplate
    {
        private VocabularTemplate()
        {
            // EF Core    
        }

        public Guid VocabularTemplateId { get; private set; }
        public string Name { get; private set; }
        public string Meaning { get; private set; }
        public string Reading { get; private set; }

        public VocabularType Type { get; private set; }
        public String MeaningMnemonic { get; private set; }
        public String ReadingMnemonic { get; private set; }
        public WordType WordType { get; private set; }

        public int ComponentsNumber { get; private set; }

        public byte RequiredLevel { get; private set; }

        public List<VocabularItem> VocabularItems { get; set; }

        public static VocabularTemplate Create(string name, string meaning, string reading, VocabularType type, byte requiredLevel, string meaningMnemonic, string readingMnemonic, WordType wordType=0)
        {
            var instance = new VocabularTemplate { VocabularTemplateId = Guid.NewGuid() };
            instance.Update(name, meaning, reading, type, requiredLevel, meaningMnemonic, readingMnemonic, wordType);
            return instance;
        }

        public void Update(string name, string meaning, string reading, VocabularType type, byte requiredLevel, string meaningMnemonic, string readingMnemonic, WordType wordType, int components=0)
        {
            Name = name;
            Meaning = meaning;
            Reading = reading;
            Type = type;
            RequiredLevel = requiredLevel;
            MeaningMnemonic = meaningMnemonic;
            ReadingMnemonic = readingMnemonic;
            WordType = wordType;
            Update(components);
        }

        public void Update(int components)
        {
            ComponentsNumber = components;
        }
    }
}
