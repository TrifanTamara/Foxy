using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface IFormularTempRepo : IGenericRepository<FormularTemplate>
    {
        //Task AddFormularsForNewUser(Guid userId);
        Task<List<FormularTemplate>> GetByType(FormularType type, String name);
    }
}
