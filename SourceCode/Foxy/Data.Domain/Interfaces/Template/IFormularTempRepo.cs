using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IFormularTempRepo : IGenericRepository<FormTemplate>
    {
        //Task AddFormularsForNewUser(Guid userId);
        Task<List<FormTemplate>> GetByType(FormType type, String name);
    }
}
