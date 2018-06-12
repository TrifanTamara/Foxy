using Business.Wrappers;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Wrappers
{
    public class QuestionWrapper
    {
        public QuestionItem Item { get; set; }
        public QuestionTemplate Template { get; set; }


        public List<VocabularWrapper> RequiredWords { get; set; }

        public QuestionWrapper(QuestionItem item, QuestionTemplate template, List<VocabularWrapper> requiredWords)
        {
            Item = item;
            Template = template;

            RequiredWords = requiredWords;

            TransformInformation();
        }

        private void TransformInformation()
        {

        }
    }
}
