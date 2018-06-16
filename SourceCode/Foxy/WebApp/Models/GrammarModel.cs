using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class GrammarModel
    {
        public List<FormularWrapper> Formulars { get; private set; }
        public GrammarModel(List<FormularWrapper> formulars)
        {
            Formulars = formulars.OrderBy(x => x.Template.PartialViewId).ToList();
        }
    }
}
