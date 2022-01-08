using System.Collections.Generic;

namespace Alerting.Tests.AcceptanceTests.Models
{
    // 400 series erros
    public class BadRequestError
    {
        public string Key { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}