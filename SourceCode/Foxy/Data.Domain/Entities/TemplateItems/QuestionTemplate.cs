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

        public Guid Id { get; private set; }

        public string Content { get; private set; }
        public string Note { get; private set; }

        public List<VocabularTemplate> VocabularTemplates { get; private set; }
        public List<AnswerTemplate> AnswerTemplates { get; private set; }

        public static QuestionTemplate Create(string content, List<VocabularTemplate> words, 
            List<AnswerTemplate> answers, string note = "")
        {
            var instance = new QuestionTemplate { Id = Guid.NewGuid() };
            instance.Update(content, words, answers, note);
            return instance;
        }

        public void Update(string content, List<VocabularTemplate> words,
            List<AnswerTemplate> answers, string note = "")
        {
            Content = content;
            VocabularTemplates = words;
            AnswerTemplates = answers;
            Note = note;
        }

        public void Update(List<VocabularTemplate> words, List<AnswerTemplate> answers)
        {
            VocabularTemplates = words;
            AnswerTemplates = answers;
        }
    }
}
