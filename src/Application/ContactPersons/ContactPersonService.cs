using Alerting.Application.Common.DomainExceptions;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Infrastructure.Data.Repositories.ContactPersons;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alerting.Application.ContactPersons
{
    public class ContactPersonService
    {
        private readonly ContactPersonRepository _repository;
        private readonly IMapper _mapper;

        public ContactPersonService(ContactPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public int GetContactPersonsCount(ContactPersonsFilter filter)
        {
            return _repository.GetContactPersonsCount(filter);
        }

        public IEnumerable<ContactPersonListItem> GetContactPersons(ContactPersonsPagableFilter filter)
        {
            var entities = _repository.GetContactPersons(filter);
            var result = entities.Select(o => _mapper.Map<ContactPersonListItem>(o));
            return result;
        }

        public ContactPerson CreateContactPerson(ContactPerson contactPerson)
        {
            _repository.CreateContactPerson(contactPerson);
            _repository.UnitOfWork.SaveChanges();
            return contactPerson;
        }

        public void DeleteContactPerson(int id)
        {
            var entity = _repository.GetContactPersonById(id);
            if (entity == null)
                throw new DomainException(ErrorStatusCode.notFound, ErrorMessage.contactPersonNotFound);

            _repository.DeleteContactPerson(entity);
            _repository.UnitOfWork.SaveChanges();
        }

        public ContactPerson UpdateContactPerson(int id, ContactPerson contactPerson)
        {
            var entity = _repository.GetContactPersonById(id);
            if (entity == null)
                throw new DomainException(ErrorStatusCode.notFound, ErrorMessage.contactPersonNotFound);

            entity = _mapper.Map(contactPerson, entity);
            _repository.UpdateContactPerson(entity);
            _repository.UnitOfWork.SaveChanges();

            return entity;
        }
    }
}