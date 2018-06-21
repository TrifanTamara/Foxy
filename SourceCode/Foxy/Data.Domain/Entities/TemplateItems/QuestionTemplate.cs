using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public class QuestionTemplate
    {
        private QuestionTemplate()
        {
            // EF Core    
        }

        public Guid QuestionTemplateId { get; private set; }

        public string Content { get; private set; }
        public string Note { get; private set; }
        
        public List<AnswerTemplate> AnswerTemplates { get; private set; }

        public FormTemplate FormTemplate;

        public static QuestionTemplate Create(string content, List<AnswerTemplate> answers, string note = "")
        {
            var instance = new QuestionTemplate { QuestionTemplateId = Guid.NewGuid() };
            instance.Update(content, note);
            instance.Update(answers);
            return instance;
        }

        public void Update(FormTemplate formularTemplate)
        {
            FormTemplate = formularTemplate;
        }

        public void Update(string content, string note = "")
        {
            Content = content;
            Note = note;
        }

        public void Update(List<AnswerTemplate> answers)
        {
            AnswerTemplates = new List<AnswerTemplate>();
            foreach (var a in answers) AnswerTemplates.Add(a);
        }
    }
}
