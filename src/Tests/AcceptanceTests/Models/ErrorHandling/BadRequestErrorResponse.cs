using System.Collections.Generic;

namespace Alerting.Tests.AcceptanceTests.Models
{
    public class BadRequestErrorResponse : ErrorResponse
    {
        public IEnumerable<BadRequestError> Errors { get; set; }
    }
}