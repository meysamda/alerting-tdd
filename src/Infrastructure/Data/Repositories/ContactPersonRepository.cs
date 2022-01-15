using System;
using System.Linq;
using Alerting.Infrastructure.Data.DbContexts;
using Alerting.Infrastructure.Data.DbContexts.Entities;

namespace Alerting.Infrastructure.Data.Repositories
{
    public class ContactPersonRepository : IRepository
    {
        private readonly AlertingDbContext _dbContext;

        public ContactPersonRepository(AlertingDbContext dbContext)
        {
            _dbContext = dbContext;    
        }

        public IUnitOfWork UnitOfWork => _dbContext;

        public void CreateContactPerson(ContactPerson contactPerson)
        {
            _dbContext.ContactPersons.Add(contactPerson);
        }

        public ContactPerson GetContactPersonById(int id)
        {
            return _dbContext.ContactPersons.FirstOrDefault(o => o.Id == id);
        }

        public void DeleteContactPerson(ContactPerson contactPerson)
        {
            _dbContext.ContactPersons.Remove(contactPerson);
        }

        public void UpdateContactPerson(ContactPerson entity)
        {
            _dbContext.ContactPersons.Update(entity);
        }
    }
}