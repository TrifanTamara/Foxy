using Business.Wrappers;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Domain.Wrappers
{
    public class FormularWrapper
    {
        public FormularItem Item { get; set; }
        public FormularTemplate Template { get; set; }

        public List<QuestionWrapper> Questions { get; set; }
        public List<VocabularWrapper> RequiredVocabular { get; set; }

        public FormularWrapper(FormularItem item, FormularTemplate template, List<QuestionWrapper> questions, List<VocabularWrapper> reqVoc)
        {
            Item = item;
            Template = template;

            Questions = questions;
            RequiredVocabular = reqVoc;

            TransformInformation();
        }

        private void TransformInformation()
        {

        }
    }
}
