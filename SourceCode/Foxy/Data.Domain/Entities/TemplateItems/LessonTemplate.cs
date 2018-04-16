using System;
using System.Collections.Generic;

namespace Data.Domain.Entities.TemplateItems
{
    public enum LessonType : byte
    {
        Grammar,
        Reading
    }

    public class LessonTemplate
    {
        private LessonTemplate()
        {
            // EF Core      
        }

        public Guid Id { get; private set; }
        public string Topic { get; private set; }
        public string Content { get; private set; }
        public LessonType Type { get; private set; }
        public byte RequiredLevel { get; private set; }
        public ICollection<VocabularTemplate> RequiredVocabular { get; private set; }

        public static LessonTemplate Create(string topic, string content, LessonType type, byte requiredLevel, ICollection<VocabularTemplate> requiredVocabular)
        {
            var instance = new LessonTemplate { Id = Guid.NewGuid() };
            instance.Update(topic, content, type, requiredLevel, requiredVocabular);
            return instance;
        }

        public void Update(string topic, string content, LessonType type, byte requiredLevel, ICollection<VocabularTemplate> requiredVocabular)
        {
            Topic = topic;
            Content = content;
            Type = type;
            RequiredLevel = requiredLevel;
            RequiredVocabular = requiredVocabular;
        }

        public void Update(ICollection<VocabularTemplate> requiredVocabular)
        {
            RequiredVocabular = requiredVocabular;
        }
    }
}
