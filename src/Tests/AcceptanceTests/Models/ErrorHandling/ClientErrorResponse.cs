using System.Collections.Generic;

namespace Alerting.Tests.AcceptanceTests.Models
{
    public class ClientErrorResponse : ErrorResponse
    {
        public string Error { get; set; }
        public string MoreDetails { get; set; }
    }
}