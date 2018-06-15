using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public enum FQAElementType : byte
    {
        Answer,
        Question,
        Formular
    }

    public class WordFormQuestAnsRel
    {
        private WordFormQuestAnsRel()
        {
            // EF Core    
        }

        public Guid MainElementId { get; private set; }
        public Guid WordId { get; private set; }
        public FQAElementType Type { get; private set; }

        public static WordFormQuestAnsRel Create(Guid main, Guid contained, FQAElementType type)
        {
            var instance = new WordFormQuestAnsRel();
            instance.Update(main, contained, type);
            return instance;
        }

        public void Update(Guid main, Guid contained, FQAElementType type)
        {
            MainElementId = main;
            WordId = contained;
            Type = type;
        }
    }
}
