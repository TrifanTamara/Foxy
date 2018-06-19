using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public interface IDatabaseContext
    {
        DbSet<User> Users { get; set; }
        DbSet<ImageEntity> Images { get; set; }

        DbSet<VocabularTemplate> VocabularTemplates { get; set; }
        DbSet<VocabularItem> VocabularItems { get; set; }
        DbSet<VocabularRelationship> VocabularRelationships { get; set; }

        DbSet<FormularTemplate> FormularTemplates { get; set; }
        DbSet<FormularItem> FormularItems { get; set; }

        DbSet<QuestionTemplate> QuestionTemplates { get; set; }
        DbSet<QuestionItem> QuestionItems { get; set; }

        DbSet<AnswerTemplate> AnswerTemplates { get; set; }

        DbSet<WordsInText> WordFormQuestAnsRels { get; set; }

        int SaveChanges();

    }
}
