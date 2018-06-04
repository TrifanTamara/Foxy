using Business.Wrappers;
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
        Task AddToLesson(VocabularItem vocab);
        Task RemoveFromLesson(VocabularItem vocab);
        Task AddAnswer(VocabularItem vocab, bool answer);
        Task PassToNextLevel(int level, Guid userId);
        Task<List<VocabularWrapper>> GetVocabLesson(Guid userId);
        Task<List<VocabularWrapper>> GetVocabForReview(Guid userId);
        bool ActiveForReview(VocabularItem item);
        Task<List<VocabularItem>> GetItemsByGrandLevels(Guid userId, GrandLevels level);
        Task<List<VocabularWrapper>> GetVocabLessonByTypes(Guid userId, int level, VocabularType type, InfoLessonType requestedInfo);
        Task<List<VocabularWrapper>> GetItemsForLesson(Guid userId);
        Task<VocabularWrapper> GetWrappedItem(Guid userId, string name, VocabularType type);
        string GrandLvlNameFromMini(MiniLevels minilevel);
    }
}
