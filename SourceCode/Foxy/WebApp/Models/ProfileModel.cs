using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ProfileModel
    {
        public User User { get; set; }
        public List<VocabularTemplate> favoriteRadical { get; set; }
        public List<VocabularTemplate> favoriteKanji { get; set; }
        public List<VocabularTemplate> favoriteWords { get; set; }

        public List<FormTemplate> favoriteGrammar { get; set; }
        public List<FormTemplate> favoriteReading { get; set; }

    }
}
