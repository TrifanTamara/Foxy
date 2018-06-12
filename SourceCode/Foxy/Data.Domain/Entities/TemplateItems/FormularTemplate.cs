using System;
using System.Collections.Generic;

namespace Data.Domain.Entities.TemplateItems
{
    public enum FormularType : byte
    {
        Grammar,
        Reading,
        Listening
    }

    public class FormularTemplate
    {
        private FormularTemplate()
        {
            // EF Core      
        }

        public Guid Id { get; private set; }
        public int PartialViewId { get; private set; }

        public string Topic { get; private set; }
        public string Description { get; private set; }
        public FormularType Type { get; private set; }
        public List<VocabularTemplate> Words { get; private set; }
        public List<QuestionTemplate> Questions { get; private set; }

        public static FormularTemplate Create(int partialViewId, string topic, string content, FormularType type, List<VocabularTemplate> words, List<QuestionTemplate> questions)
        {
            var instance = new FormularTemplate { Id = Guid.NewGuid() };
            instance.Update(partialViewId, topic, content, type, words, questions);
            return instance;
        }

        public void Update(int partialViewId, string topic, string content, FormularType type, List<VocabularTemplate> words, List<QuestionTemplate> questions)
        {
            Topic = topic;
            Description = content;
            Type = type;
            Words = words;
            Questions = questions;
            PartialViewId = partialViewId;
        }

        public void Update(List<VocabularTemplate> words, List<QuestionTemplate> questions)
        {
            Words = words;
            Questions = questions;
        }
    }
}
