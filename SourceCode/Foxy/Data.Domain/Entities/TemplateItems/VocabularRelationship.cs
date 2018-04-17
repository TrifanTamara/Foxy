using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Entities.TemplateItems
{
    public class VocabularRelationship
    {
        private VocabularRelationship()
        {
            // EF Core    
        }
        
        public Guid MainItemId { get; private set; }
        public Guid ContainedItemId { get; private set; }

        public static VocabularRelationship Create(Guid main, Guid contained)
        {
            var instance = new VocabularRelationship();
            instance.Update(main, contained);
            return instance;
        }

        public void Update(Guid main, Guid contained)
        {
            MainItemId = main;
            ContainedItemId = contained;
        }
    }
}
