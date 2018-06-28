using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ReadListModel
    {
        public List<FormularWrapper> ReadingForms { get; private set; }
        public List<FormularWrapper> ListeningForms { get; private set; }
        public ReadListModel(List<FormularWrapper> read, List<FormularWrapper> list)
        {
            ReadingForms = read.OrderBy(x => x.Template.PartialViewId).ToList();
            ListeningForms = list.OrderBy(x => x.Template.PartialViewId).ToList();  
        }
    }
}
