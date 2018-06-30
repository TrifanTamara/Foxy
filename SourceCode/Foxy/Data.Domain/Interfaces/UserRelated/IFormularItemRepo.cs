using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.UserRelated
{
    public interface IFormularItemRepo : IGenericRepository<FormItem>
    {
        Task<FormularWrapper> GetWrappedItem(Guid userId, Guid formularTemplateId);
        Task<FormItem> GetItemByTemplateAndUser(Guid userId, Guid templateId);
        Task AddItemsForUser(Guid userId);
        Task<List<FormularWrapper>> WrapFormularList(List<FormItem> formulars);
        Task<List<FormularWrapper>> WrapFormularList(List<FormTemplate> formulars, Guid userId);

        Task<List<FormularWrapper>> GetAllFormularsByUser(Guid userId);
        Task<List<FormularWrapper>> GetAllFormByUserAndType(Guid userId, FormType type);
        Task<FormularWrapper> GetByUserAndPvId(Guid userId, int pvId, FormType type);
        Task<List<QuestionWrapper>> GetQuestionsByUserAndPvId(Guid userId, int pvId, FormType type);

        Task<List<FormTemplate>> GetFavoriteFormsType(Guid userId, FormType type);

        Task<List<FormTemplate>> GetAllFormTempByType(FormType type);
    }
}
