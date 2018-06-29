using Business.Wrappers;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

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
        public List<VocabularWrapper> WordsPassed { get; set; }
        public List<VocabularWrapper> WordsTotal { get; set; }


        public int KanjiPercent { get; set; }
        public int RadicalPercent { get; set; }
        public int WordsPercent { get; set; }

        public GrammarModel Grammar { get; set; }
        public ReadListModel Reading { get; set; }

        public void CalculatePercentages()
        {
            int total;
            if (RadicalsLesson != null)
            {
                total = RadicalsLesson.Count() + RadicalsViewed.Count() + RadicalsPassed.Count();
                RadicalPercent = (int)(RadicalsPassed.Count() * 100 / (float)total);
            }
            if (KanjisLesson != null)
            {
                total = KanjisLesson.Count() + KanjisViewed.Count() + KanjisPassed.Count();
                KanjiPercent = (int)(KanjisPassed.Count() * 100 / (float)total);
            }
            if (WordsLesson != null)
            {
                total = WordsTotal.Count();
                if (total == 0) WordsPercent = 100;
                else WordsPercent = ((int)((WordsPassed.Count() / (float)total) * 100));
            }
        }
    }
}
