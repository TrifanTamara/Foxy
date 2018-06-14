using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public enum InfoRequested
    {
        AllInfo,
        JustRadical,
        JustKanji,
        JustWords,
        Progress
    }

    public class VocabLevelsModel
    {
        public List<LevelWrapper> Levels { get; set; }
        public InfoRequested RequestedInfo { get; set; }
    }
}
