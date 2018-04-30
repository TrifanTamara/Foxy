using System;

namespace Data.Domain.Entities.TemplateItems
{
    public enum VocabularType : byte
    {
        Radical,
        Kanji,
        Word
    }

    public class VocabularTemplate
    {
        private VocabularTemplate()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Meaning { get; private set; }
        public string Reading { get; private set; }
        public VocabularType Type { get; private set; }
        public String Mnemonic { get; private set; }
        public byte RequiredLevel { get; private set; }

        public static VocabularTemplate Create(string name, string meaning, string reading, VocabularType type, byte requiredLevel, string mention)
        {
            var instance = new VocabularTemplate { Id = Guid.NewGuid() };
            instance.Update(name, meaning, reading, type, requiredLevel, mention);
            return instance;
        }

        public void Update(string name, string meaning, string reading, VocabularType type, byte requiredLevel, string mnemonic)
        {
            Name = name;
            Meaning = meaning;
            Reading = reading;
            Type = type;
            RequiredLevel = requiredLevel;
            Mnemonic = mnemonic;
        }
    }
}
