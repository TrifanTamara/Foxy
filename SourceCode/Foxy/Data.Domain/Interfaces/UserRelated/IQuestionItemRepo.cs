using Data.Domain.Entities.UserRelated;
using Data.Domain.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.UserRelated
{
    public interface IQuestionItemRepo : IGenericRepository<QuestionItem>
    {
        Task<QuestionWrapper> GetWrappedItem(Guid userId, Guid questionTemplateId);
        Task<QuestionItem> GetItemByTemplate(Guid templateId);
        Task AddItemsForUser(Guid userId);
    }
}
