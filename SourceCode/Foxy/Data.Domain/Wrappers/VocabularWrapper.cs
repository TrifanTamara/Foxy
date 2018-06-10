using Data.Domain.Entities;
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

        public string MainMeaning { get; set; }
        public string MainReading { get; set; }
        
        public string VocabularType { get; set; }
        public string WordTypeString { get; set; }

        public bool Unlocked { get; set; }
        public bool OnyomyIsMain { get; set; }
        public string LastTimeAString { get; set; }
        public string TimeUntilNextReview { get; set; }
        public int Percent { get; set; }
        public string ItemLevel { get; set; }

        public VocabularWrapper(VocabularItem item, VocabularTemplate template, List<VocabularTemplate> components, string level)
        {
            Item = item;
            Template = template;
            Components = components;
            ItemLevel = level;

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
                    if (mainMeaning)
                    {
                        OnyomyIsMain = false;
                        MainReading = KunyoumiReading[0];
                    }
                }
                else
                {
                    OnyomiReading = new List<string>(value.Split(";"));
                    if (mainMeaning) {
                        OnyomyIsMain = true;
                        MainReading = OnyomiReading[0];
                    }
                }
            }
        }

        private void TransformInformation()
        {
            Name = Template.Name;

            MainMeaning = Template.Meaning.Split(";")[0];

            MeaningsList = new List<string>(Template.Meaning.Split(";"));

            if (Item.UserSynonyms.Equals("")) UserSynonyms = new List<string>();
            else UserSynonyms = new List<string>(Item.UserSynonyms.Split(";"));

            OnyomiReading = new List<string>();
            KunyoumiReading = new List<string>();

            if (Item.RightAnswers + Item.WrongAnswers == 0) Percent = 0;
            else Percent = (int)((Item.RightAnswers / (float)(Item.RightAnswers + Item.WrongAnswers)*100));

            LastTimeAString = "x";
            Unlocked = DateTime.Compare(Item.UnlockTime, DateTime.Now) <= 0;
            if (!Unlocked)
            {
                LastTimeAString = "None";
                TimeUntilNextReview = "Locked";
            }
            else if (Unlocked && DateTime.Compare(Item.LastTimeAnswered, DateTime.Now) > 0)
            {
                LastTimeAString = "None";
                TimeUntilNextReview = "Active in Lesson";
            }
            else
            {
                TimeSpan span = DateTime.Now - Item.LastTimeAnswered;
                LastTimeAString = FormatTime(span);
                if (LastTimeAString.Equals("now")) TimeUntilNextReview = "Just now";
                else LastTimeAString = "about " + LastTimeAString + " ago";

                span = Item.LastTimeAnswered.AddMinutes(StaticInfo.minutesForLevel[(int)Item.CurrentMiniLevel]) - DateTime.Now; 
                TimeUntilNextReview = FormatTime(span);
                if (TimeUntilNextReview.Equals("now")) TimeUntilNextReview = "Active in Review";
                else TimeUntilNextReview = "in " + TimeUntilNextReview;
            }

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

        public string FormatTime(TimeSpan span)
        {
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("{0} {1}",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("{0} {1}",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("{0} {1}",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("{0} {1}",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("{0} {1}",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("{0} seconds", span.Seconds);
            if (span.Seconds <= 5)
                return "now";
            return string.Empty;
        }
    }
}