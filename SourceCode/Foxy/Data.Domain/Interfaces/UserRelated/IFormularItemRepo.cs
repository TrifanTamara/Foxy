using Data.Domain.Entities.TemplateItems;
using Data.Domain.Entities.UserRelated;
using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.UserRelated
{
    public interface IFormularItemRepo : IGenericRepository<FormularItem>
    {
        Task<FormularWrapper> GetWrappedItem(Guid userId, Guid formularTemplateId);
        Task<FormularItem> GetItemByTemplate(Guid templateId);
        Task AddItemsForUser(Guid userId);
        Task<List<FormularWrapper>> WrapFormularList(List<FormularItem> formulars);
        Task<List<FormularWrapper>> GetAllFormularsByUser(Guid userId);
        Task<List<FormularWrapper>> GetAllFormByUserAndType(Guid userId, FormularType type);
        Task<FormularWrapper> GetByUserAndPvId(Guid userId, int pvId);
    }
}
