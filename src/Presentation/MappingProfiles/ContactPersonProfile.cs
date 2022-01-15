using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Presentation.Commands;
using AutoMapper;

namespace Alerting.Presentation.MappingProfiles
{
    public class ContactPersonProfile : Profile
    {
        public ContactPersonProfile()
        {
            CreateMap<CreateContactPersonCommand, ContactPerson>();
            CreateMap<UpdateContactPersonCommand, ContactPerson>();
        }
    }
}