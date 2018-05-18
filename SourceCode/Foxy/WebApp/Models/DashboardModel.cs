using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class DashboardModel
    {
        public int CurrentLevel { get; set; }

        public int SeedLevelNr { get; set; }
        public int LeafLevelNr { get; set; }
        public int BloomLevelNr { get; set; }
        public int FlourishedLevelNr { get; set; }

        public List<VocabularItem> RadicalsLesson { get; set; }
        public List<VocabularItem> RadicalsViewed { get; set; }
        public List<VocabularItem> RadicalsPassed { get; set; }

        public List<VocabularItem> KanjisLesson { get; set; }
        public List<VocabularItem> KanjisViewed { get; set; }
        public List<VocabularItem> KanjisPassed { get; set; }

        public List<VocabularItem> Words { get; set; }
    }
}
