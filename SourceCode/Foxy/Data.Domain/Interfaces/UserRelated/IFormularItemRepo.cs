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
        Task<FormItem> GetItemByTemplate(Guid templateId);
        Task AddItemsForUser(Guid userId);
        Task<List<FormularWrapper>> WrapFormularList(List<FormItem> formulars);
        Task<List<FormularWrapper>> GetAllFormularsByUser(Guid userId);
        Task<List<FormularWrapper>> GetAllFormByUserAndType(Guid userId, FormType type);
        Task<FormularWrapper> GetByUserAndPvId(Guid userId, int pvId);
    }
}
