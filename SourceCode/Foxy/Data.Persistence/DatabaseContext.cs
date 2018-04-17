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
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<VocabularTemplate> VocabularTemplates { get; set; }
        public DbSet<VocabularItem> VocabularItems { get; set; }
        public DbSet<VocabularRelationship> VocabularRelationships { get; set; }

        public DbSet<LessonTemplate> LessonTemplates { get; set; }
        public DbSet<LessonItem> LessonItems { get; set; }

        public DbSet<QuizzTemplate> QuizzTemplates { get; set; }
        public DbSet<QuizzItem> QuizzItems { get; set; }
        
    }
}
