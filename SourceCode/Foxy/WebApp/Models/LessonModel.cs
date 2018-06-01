using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class LessonModel
    {
        public List<VocabularWrapper> LessonList { get; set; }
        public List<bool> ItemVisited { get; set; }

        public int CurrentIndex { get; set; }
        public bool ReviewActive { get; set; }
    }
}
