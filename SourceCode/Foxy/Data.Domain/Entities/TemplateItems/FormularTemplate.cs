using System;
using System.Collections.Generic;

namespace Data.Domain.Entities.TemplateItems
{
    public enum FormularType : byte
    {
        Grammar,
        Reading
    }

    public class FormularTemplate
    {
        private FormularTemplate()
        {
            // EF Core      
        }

        public Guid Id { get; private set; }
        public string Topic { get; private set; }
        public string Content { get; private set; }
        public string Note { get; private set; }
        public FormularType Type { get; private set; }
        public List<VocabularTemplate> Words { get; private set; }
        public List<QuestionTemplate> Questions { get; private set; }

        public static FormularTemplate Create(string topic, string content, FormularType type, List<VocabularTemplate> words, List<QuestionTemplate> questions, string note = "")
        {
            var instance = new FormularTemplate { Id = Guid.NewGuid() };
            instance.Update(topic, content, type, words, questions, note);
            return instance;
        }

        public void Update(string topic, string content, FormularType type, List<VocabularTemplate> words, List<QuestionTemplate> questions, string note = "")
        {
            Topic = topic;
            Content = content;
            Type = type;
            Words = words;
            Questions = questions;
            Note = note;
        }

        public void Update(List<VocabularTemplate> words, List<QuestionTemplate> questions)
        {
            Words = words;
            Questions = questions;
        }
    }
}
