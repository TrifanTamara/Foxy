using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public enum InfoLessonType
    {
        ActiveLesson,
        ViewedLesson,
        Passed,
        All
    }

    public interface IVocabularItemRepository : IGenericRepository<Entities.UserRelated.VocabularItem>
    {
        Task AddVocabularForNewUser(Guid userId);
        Task<IEnumerable<VocabularItem>> GetVocabByUser(Guid userId);
        Task<VocabularItem> GetVocabByTemplate(Guid templateId);
        Task UnlockElements(VocabularItem item);
        Task AddToLesson(VocabularItem vocab, VocabularTemplate vTemp);
        Task RemoveFromLesson(VocabularItem vocab);
        Task AddAnswer(VocabularItem vocab, bool answer);
        Task PassToNextLevel(int level, Guid userId);
        Task<List<VocabularItem>> GetVocabLesson(Guid userId);
        Task<List<VocabularItem>> GetVocabForReview(Guid userId);
        bool ActiveForReview(VocabularItem item);
        Task<List<VocabularItem>> GetItemsByGrandLevels(Guid userId, GrandLevels level);
        Task<List<VocabularItem>> GetVocabLessonByTypes(Guid userId, int level, VocabularType type, InfoLessonType requestedInfo);
    }
}
