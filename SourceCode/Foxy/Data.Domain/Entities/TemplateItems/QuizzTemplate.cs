using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public class QuizzTemplate
    {
        private QuizzTemplate()
        {
            // EF Core    
        }

        public Guid Id { get; private set; }
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public LessonType Type { get; private set; }
        public ICollection<LessonTemplate> RequiredLessons { get; private set; }
        public ICollection<VocabularTemplate> RequiredVocabular { get; private set; }

        public static QuizzTemplate Create(string question, string answer, LessonType type,
            ICollection<LessonTemplate> requiredLessons,
            ICollection<VocabularTemplate> requiredVocabular)
        {
            var instance = new QuizzTemplate { Id = Guid.NewGuid() };
            instance.Update(question, answer, type, requiredLessons, requiredVocabular);
            return instance;
        }

        public void Update(string question, string answer, LessonType type,
            ICollection<LessonTemplate> requiredLessons,
            ICollection<VocabularTemplate> requiredVocabular)
        {
            Question = question;
            Answer = answer;
            Type = type;
            RequiredLessons = requiredLessons;
            RequiredVocabular = requiredVocabular;
        }

        public void Update(ICollection<VocabularTemplate> requiredVocabular)
        {
            RequiredVocabular = requiredVocabular;
        }
    }
}
