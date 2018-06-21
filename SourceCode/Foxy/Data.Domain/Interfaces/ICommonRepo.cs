using Data.Domain.Entities.TemplateItems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces
{
    public interface ICommonRepo
    {        Task SaveFormular(FormTemplate formular);
    }
}
