using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Business
{
    public class UsersRepo :
        GenericRepo<User>, IUsersRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public UsersRepo(IDatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _databaseContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email));
        }
    }
}
