using System;
using System.Collections.Generic;
using System.Text;
using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Business
{
    public class VocabRelRepository :
        GenericRepository<VocabularRelationship>, IVocabRelRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private DbSet<VocabularRelationship> _entities;


        public VocabRelRepository(IDatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }
    }
}
