using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public class AnswerTemplate
    {
        private AnswerTemplate()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public string Note { get; private set; }
        public bool IsTrue { get; private set; }


        public List<VocabularTemplate> Words { get; private set; }


        public static AnswerTemplate Create(string text, bool isTrue, List<VocabularTemplate> words, string note = "")
        {
            var instance = new AnswerTemplate { Id = Guid.NewGuid() };
            instance.Update(text, isTrue, note);
            instance.Update(words);
            return instance;
        }

        public void Update(string text, bool isTrue, string note = "")
        {
            Text = text;
            IsTrue = isTrue;
            Note = note;
        }

        public void Update(List<VocabularTemplate> words)
        {
            Words = words;
        }
    }
}
