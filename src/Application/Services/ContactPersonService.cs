using System.Threading.Tasks;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Infrastructure.Data.Repositories;

namespace Alerting.Application.Services
{
    public class ContactPersonService
    {
        private readonly ContactPersonRepository _repository;

        public ContactPersonService(ContactPersonRepository repository)
        {
            _repository = repository;
        }

        public int CreateContactPerson(ContactPerson contactPerson)
        {
            _repository.CreateContactPerson(contactPerson);
            _repository.UnitOfWork.SaveChanges();
            return contactPerson.Id;
        }
    }
}