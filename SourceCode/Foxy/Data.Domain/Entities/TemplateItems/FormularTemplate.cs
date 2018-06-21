using System;
using System.Collections.Generic;

namespace Data.Domain.Entities.TemplateItems
{
    public enum FormType : byte
    {
        Grammar,
        Reading,
        Listening
    }

    public class FormTemplate
    {
        private FormTemplate()
        {
            // EF Core      
        }

        public Guid FormTemplateId { get; private set; }
        public int PartialViewId { get; private set; }

        public string Topic { get; private set; }
        public string Description { get; private set; }
        public bool Seen { get; private set; }

        public FormType Type { get; private set; }
        public virtual List<QuestionTemplate> QuestionTemplates { get; private set; }

        public static FormTemplate Create(int partialViewId, string topic, string content, FormType type, List<QuestionTemplate> questions)
        {
            var instance = new FormTemplate { FormTemplateId = Guid.NewGuid() };
            instance.Update(partialViewId, topic, content, type);
            instance.Update(questions);
            return instance;
        }

        public void Update(int partialViewId, string topic, string content, FormType type)
        {
            Topic = topic;
            Description = content;
            Type = type;
            PartialViewId = partialViewId;
            Seen = false;
        }

        public void Update(List<QuestionTemplate> questions)
        {
            QuestionTemplates = new List<QuestionTemplate>();
            foreach (var q in questions) QuestionTemplates.Add(q);
        }
    }
}
