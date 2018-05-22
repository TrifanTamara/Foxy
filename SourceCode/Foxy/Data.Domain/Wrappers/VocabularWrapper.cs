using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Wrappers
{
    public class VocabularWrapper
    {
        public VocabularItem Item { get; set; }
        public VocabularTemplate Template {get; set;}
        public VocabularWrapper(VocabularItem item, VocabularTemplate template)
        {
            Item = item;
            Template = template;
        }
    }
}
