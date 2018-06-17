using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class QuizzModel
    {
        public List<QuestionWrapper> Questions {get; private set;}
        public FormularWrapper Formular { get; private set; }


        public QuizzModel(List<QuestionWrapper> quest, FormularWrapper form)
        {
            Questions = quest;
            Formular = form;
        }
    }
}
