using Business.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class ReviewModel
    {
        public List<VocabularWrapper> Reviewitems { get; set; }

        public int CurrentIndex { get; set; }
    }
}
