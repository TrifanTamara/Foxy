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

        public Guid AnswerTemplateId { get; private set; }
        public string Text { get; private set; }
        public string Note { get; private set; }
        public bool IsTrue { get; private set; }

        public static AnswerTemplate Create(string text, bool isTrue, string note = "")
        {
            var instance = new AnswerTemplate { AnswerTemplateId = Guid.NewGuid() };
            instance.Update(text, isTrue, note);
            return instance;
        }

        public void Update(string text, bool isTrue, string note = "")
        {
            Text = text;
            IsTrue = isTrue;
            Note = note;
        }
    }
}
