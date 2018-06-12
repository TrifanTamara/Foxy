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
    }
}
