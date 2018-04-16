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

        public Guid Id { get; private set; }
        public VocabularTemplate MainItem { get; private set; }
        public VocabularTemplate ContainedItem { get; private set; }

        public static VocabularRelationship Create(VocabularTemplate main, VocabularTemplate contained)
        {
            var instance = new VocabularRelationship() { Id = Guid.NewGuid() };
            instance.Update(main, contained);
            return instance;
        }

        public void Update(VocabularTemplate main, VocabularTemplate contained)
        {
            MainItem = main;
            ContainedItem = contained;
        }
    }
}
