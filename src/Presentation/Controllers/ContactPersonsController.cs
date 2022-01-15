using System.ComponentModel.DataAnnotations;
using Alerting.Application.ContactPersons;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Presentation.Attributes;
using Alerting.Presentation.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/contact-persons")]
    [ApiController]
    public class ContactPersonsController : ControllerBase
    {
        private readonly ContactPersonService _service;
        private readonly IMapper _mapper;

        public ContactPersonsController(ContactPersonService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// create contact person
        /// </summary>
        /// <param name="command"></param>
        [Auth("AlertingAdmin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<int> CreateContactPerson([FromBody] CreateContactPersonCommand command)
        {
            var contactPerson = _mapper.Map<ContactPerson>(command);
            var result = _service.CreateContactPerson(contactPerson);
            return Ok(result.Id);
        }

        /// <summary>
        /// update contact person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        [Auth("AlertingAdmin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult UpdateContactPerson([FromRoute, Required] int id, [FromBody] UpdateContactPersonCommand command)
        {
            var updatedContactPerson = _mapper.Map<ContactPerson>(command);
            _service.UpdateContactPerson(id, updatedContactPerson);
            return NoContent();
        }

        /// <summary>
        /// delete a contact person
        /// </summary>
        /// <param name="id"></param>
        [Auth("AlertingAdmin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult DeleteContactPerson([FromRoute, Required] int id)
        {
            _service.DeleteContactPerson(id);
            return NoContent();
        }
    }
}