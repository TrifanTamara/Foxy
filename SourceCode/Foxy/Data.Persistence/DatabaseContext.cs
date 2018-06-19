using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public sealed class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VocabularRelationship>()
                .HasKey(vocabularRelationship => new { vocabularRelationship.MainItemId, vocabularRelationship.ContainedItemId });

            modelBuilder.Entity<WordsInText>()
                .HasKey(wordFormQuestAnsRel => new { wordFormQuestAnsRel.MainElementId, wordFormQuestAnsRel.WordId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<VocabularTemplate> VocabularTemplates { get; set; }
        public DbSet<VocabularItem> VocabularItems { get; set; }
        public DbSet<VocabularRelationship> VocabularRelationships { get; set; }

        public DbSet<FormularTemplate> FormularTemplates { get; set; }
        public DbSet<FormularItem> FormularItems { get; set; }

        public DbSet<QuestionTemplate> QuestionTemplates { get; set; }
        public DbSet<QuestionItem> QuestionItems { get; set; }

        public DbSet<AnswerTemplate> AnswerTemplates { get; set; }
        
        public DbSet<WordsInText> WordFormQuestAnsRels { get; set; }

    }
}
