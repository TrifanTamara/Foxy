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
        public List<string> UserSynonyms { get; set; }
        //reading
        public List<string> KunyoumiReading { get; set; }
        public List<string> OnyomiReading { get; set; }
        public string MainReading { get; set; }
        
        public string VocabularType { get; set; }
        public string WordTypeString { get; set; }

        public bool Unlocked { get; set; }
        public string LastTimeAString { get; set; }
        public int Percent { get; set; }

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
            if (Item.UserSynonyms.Equals("")) UserSynonyms = new List<string>();
            else UserSynonyms = new List<string>(Item.UserSynonyms.Split(";"));

            OnyomiReading = new List<string>();
            KunyoumiReading = new List<string>();

            if (Item.RightAnswers + Item.WrongAnswers == 0) Percent = 0;
            else Percent = (int)(Item.RightAnswers / (Item.RightAnswers + Item.WrongAnswers));

            LastTimeAString = "x";
            Unlocked = DateTime.Compare(Item.UnlockTime, DateTime.Now) <= 0;
            if (!Unlocked) LastTimeAString = "Locked";
            else if (Unlocked && DateTime.Compare(Item.LastTimeAnswered, DateTime.Now) > 0)
                LastTimeAString = "Active in Lesson";
            else LastTimeAString = TimeAgo(Item.LastTimeAnswered);

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

        public string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
    }
}