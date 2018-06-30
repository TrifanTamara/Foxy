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
        public FormItem Item { get; set; }
        public FormTemplate Template { get; set; }

        //public List<QuestionWrapper> Questions { get; set; }
        public List<VocabularWrapper> RequiredVocabular { get; set; }

        public float WordsPercentage { get; set; }
        public List<VocabularWrapper> LearnedWords { get; set; }
        public List<VocabularWrapper> NotLearnedWords { get; set; }
        public int StarsNumber { get; set; }
        public string Average { get; set; }

        public FormularWrapper(FormItem item, FormTemplate template, List<VocabularWrapper> reqVoc)
        {
            Item = item;
            Template = template;

            //Questions = questions;
            RequiredVocabular = reqVoc;

            TransformInformation();
        }

        private void TransformInformation()
        {
            LearnedWords = new List<VocabularWrapper>();
            NotLearnedWords = new List<VocabularWrapper>();
            foreach(var word in RequiredVocabular)
            {
                if (word.Item.CurrentMiniLevel >= MiniLevels.Leaf1)
                    LearnedWords.Add(word);
                else NotLearnedWords.Add(word);
            }
            WordsPercentage = (int)(LearnedWords.Count() * 100 / (float)RequiredVocabular.Count());

            StarsNumber = (int)Math.Round(Item.AverageScore*10);
            Average = String.Format("{0:0.00}", Item.AverageScore*10);
        }
    }
}
