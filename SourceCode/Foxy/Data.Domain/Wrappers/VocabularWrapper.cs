using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Wrappers
{
    public class VocabularWrapper
    {
        public VocabularItem Item { get; set; }
        public VocabularTemplate Template {get; set;}


        public List<VocabularTemplate> Components { get; set; }

        public string Name { get; set; }
        public List<string> MeaningsList { get; set; }
        //reading
        public List<string> KunyoumiReading { get; set; }
        public List<string> OnyomiReading { get; set; }
        public string MainReading { get; set; }
        
        public string VocabularType;
        public string WordTypeString;

        public VocabularWrapper(VocabularItem item, VocabularTemplate template, List<VocabularTemplate> components)
        {
            Item = item;
            Template = template;
            Components = components;

            TransformInformation();
        }

        private void AssignReading(string reading, bool mainMeaning)
        {
            List<string> aux = new List<string>(reading.Split("="));
            if (aux.Count >= 2)
            {
                string value = aux[1];
                if (reading.StartsWith("k"))
                {
                    KunyoumiReading = new List<string>(value.Split(";"));
                    if (mainMeaning) MainReading = KunyoumiReading[0];
                }
                else
                {
                    OnyomiReading = new List<string>(value.Split(";"));
                    if (mainMeaning) MainReading = OnyomiReading[0];
                }
            }
        }

        private void TransformInformation()
        {
            Name = Template.Name;
            MeaningsList = new List<string>(Template.Meaning.Split(";"));

            OnyomiReading = new List<string>();
            KunyoumiReading = new List<string>();

            List<string> aux = new List<string>(Template.Reading.Split("||"));
            if (aux.Count >= 1)
            {
                AssignReading(aux[0], true);
            }
            if (aux.Count >= 2)
            {
                AssignReading(aux[1], false);
            }

            switch (Template.Type)
            {
                case Data.Domain.Entities.TemplateItems.VocabularType.Radical:
                    VocabularType = "radical";
                    break;
                case Data.Domain.Entities.TemplateItems.VocabularType.Kanji:
                    VocabularType = "kanji";
                    break;
                case Data.Domain.Entities.TemplateItems.VocabularType.Word:
                    VocabularType = "word";
                    break;
            }
            

            if (Template.Type == Data.Domain.Entities.TemplateItems.VocabularType.Word) {
                switch (Template.WordType)
                {
                    case WordParticularType.IAdj:
                        WordTypeString = "i-adj";
                        break;
                    case WordParticularType.NaAdj:
                        WordTypeString = "na-adj";
                        break;
                    case WordParticularType.Verbs:
                        WordTypeString = "verb";
                        break;
                    case WordParticularType.Expression:
                        WordTypeString = "expression";
                        break;
                    case WordParticularType.Noun:
                        WordTypeString = "noun";
                        break;
                    case WordParticularType.Common:
                        WordTypeString = "common";
                        break;
                    case WordParticularType.Numeral:
                        WordTypeString = "numeral";
                        break;
                }
            }
        }
    }
}