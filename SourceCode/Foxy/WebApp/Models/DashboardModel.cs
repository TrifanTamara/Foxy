using Business.Wrappers;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models
{
    public class DashboardModel
    {
        public int CurrentLevel { get; set; }

        public int SeedLevelNr { get; set; }
        public int LeafLevelNr { get; set; }
        public int BloomLevelNr { get; set; }
        public int FlourishedLevelNr { get; set; }

        public List<VocabularWrapper> RadicalsLesson { get; set; }
        public List<VocabularWrapper> RadicalsViewed { get; set; }
        public List<VocabularWrapper> RadicalsPassed { get; set; }

        public List<VocabularWrapper> KanjisLesson { get; set; }
        public List<VocabularWrapper> KanjisViewed { get; set; }
        public List<VocabularWrapper> KanjisPassed { get; set; }

        public List<VocabularWrapper> WordsLesson { get; set; }
        public List<VocabularWrapper> WordsViewed { get; set; }
        public List<VocabularWrapper> WordsPassed { get; set; }


        public int KanjiPercent { get; set; }
        public int RadicalPercent { get; set; }
        public int WordsPercent { get; set; }

        public void CalculatePercentages()
        {
            if (RadicalsLesson != null)
            {
                int total = RadicalsLesson.Count() + RadicalsViewed.Count() + RadicalsPassed.Count();
                RadicalPercent = ((int)((RadicalsPassed.Count() / total) * 100));
            }
            if (KanjisLesson != null)
            {
                int total = KanjisLesson.Count() + KanjisViewed.Count() + KanjisPassed.Count();
                KanjiPercent = ((int)((RadicalsPassed.Count() / total) * 100));
            }
            if (WordsLesson != null)
            {
                int total = WordsLesson.Count() + WordsViewed.Count() + WordsPassed.Count();
                if (total == 0) WordsPercent = 100;
                else WordsPercent = ((int)((RadicalsPassed.Count() / total) * 100));
            }
        }
    }
}
