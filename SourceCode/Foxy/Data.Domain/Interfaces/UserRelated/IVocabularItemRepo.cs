using Business.Wrappers;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Wrappers;
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

    public interface IVocabularItemRepo : IGenericRepository<Entities.UserRelated.VocabularItem>
    {
        Task AddVocabularForNewUser(Guid userId);
        Task<IEnumerable<VocabularItem>> GetVocabByUser(Guid userId);
        Task<VocabularTemplate> GetVocabTemplate(Guid templateId);
        Task UnlockElements(VocabularItem item);
        Task AddToLesson(VocabularItem vocab);
        Task RemoveFromLesson(VocabularItem vocab);
        Task AddAnswer(VocabularItem vocab, bool answer);
        Task PassToNextLevel(int level, Guid userId);
        Task<List<VocabularWrapper>> GetVocabLesson(Guid userId);
        Task<List<VocabularWrapper>> GetVocabForReview(Guid userId);
        Task<List<VocabularItem>> GetItemsByGrandLevels(Guid userId, GrandLevels level);
        Task<List<VocabularWrapper>> GetVocabLessonByTypes(Guid userId, int level, VocabularType type, InfoLessonType requestedInfo);
        Task<List<VocabularWrapper>> GetItemsForLesson(Guid userId);
        Task<VocabularWrapper> GetWrappedItem(Guid userId, string name, VocabularType type);
        Task<List<VocabularWrapper>> GetAllVocabByItemType(Guid userId, VocabularType type, int level = -1);
        Task IsUserReadyForNextLevel(Guid userId);
        Task<List<LevelWrapper>> GetGroupedVocabLevels(Guid userId, int startLevel = 0, int endLevel = 0);
        Task<List<VocabularWrapper>> WrapVocabular(List<VocabularItem> oldItems);
        Task<VocabularItem> GetVocabByTemplateAndUser(Guid templateId, Guid userId);
        int GetTotalLevelNr();
        string GrandLvlNameFromMini(MiniLevels minilevel);
        bool ActiveForReview(VocabularItem item);
        Task<List<VocabularTemplate>> GetFavItems(Guid userId, VocabularType type);
    }
}
