using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Entities;

namespace Data.Domain.Interfaces
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
}
