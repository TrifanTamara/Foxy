using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ProgressVocabModel
    {
        public List<VocabularWrapper> AllVocabular { get; set; }
        public List<VocabularWrapper> Radical { get; set; }
        public List<VocabularWrapper> Kanji { get; set; }
        public List<VocabularWrapper> Words { get; set; }

        public List<VocabularWrapper> OrderListByProgress(List<VocabularWrapper> totalList)
        {
            List<VocabularWrapper> lockedList = totalList.Where(v => v.Unlocked == false).ToList();
            List<VocabularWrapper> unlockedList = totalList.Where(v => v.Unlocked == true).ToList().OrderBy(v => v.Item.CurrentMiniLevel).ToList();

            unlockedList.AddRange(lockedList);
            return unlockedList;
        }

    }
}
