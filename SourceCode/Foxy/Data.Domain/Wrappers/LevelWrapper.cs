using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Wrappers
{
    public class LevelWrapper
    {
        public int LevelNr { get; set; }
        public List<VocabularWrapper> Vocabular { get; set; }
        public List<VocabularWrapper> Kanjis { get; set; }
        public List<VocabularWrapper> Radicals { get; set; }
        public List<VocabularWrapper> Words { get; set; }

        public LevelWrapper(int levelnr, List<VocabularWrapper> vocab)
        {
            LevelNr = levelnr;
            Vocabular = vocab;
            Transform();
        }

        private void Transform()
        {
            Kanjis = new List<VocabularWrapper>();
            Radicals = new List<VocabularWrapper>();
            Words = new List<VocabularWrapper>();

            foreach(var vocab in Vocabular)
            {
                if(vocab.Template.Type == Entities.TemplateItems.VocabularType.Radical)
                {
                    Radicals.Add(vocab);
                }
                if (vocab.Template.Type == Entities.TemplateItems.VocabularType.Kanji)
                {
                    Kanjis.Add(vocab);
                }
                if (vocab.Template.Type == Entities.TemplateItems.VocabularType.Word)
                {
                    Words.Add(vocab);
                }
            }
        }
    }
}
