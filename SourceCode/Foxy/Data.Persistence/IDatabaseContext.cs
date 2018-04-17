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

        DbSet<LessonTemplate> LessonTemplates { get; set; }
        DbSet<LessonItem> LessonItems { get; set; }

        DbSet<QuizzTemplate> QuizzTemplates { get; set; }
        DbSet<QuizzItem> QuizzItems { get; set; }

        int SaveChanges();
    }
}
