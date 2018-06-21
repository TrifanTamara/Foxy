using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public enum TextType : byte
    {
        Answer,
        Question,
        Form
    }

    public class WordsInText
    {
        private WordsInText()
        {
            // EF Core    
        }

        public Guid MainElementId { get; private set; }
        public Guid WordId { get; private set; }
        public TextType Type { get; private set; }

        public static WordsInText Create(Guid main, Guid contained, TextType type)
        {
            var instance = new WordsInText();
            instance.Update(main, contained, type);
            return instance;
        }

        public void Update(Guid main, Guid contained, TextType type)
        {
            MainElementId = main;
            WordId = contained;
            Type = type;
        }
    }
}
