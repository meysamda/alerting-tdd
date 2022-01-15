namespace Alerting.Tests.AcceptanceTests.ContactPersons
{
    public class UpdateContactPersonCommand : CreateContactPersonCommand
    {
        public UpdateContactPersonCommand(string firstName, string lastName, string phoneNumber, string email) : base(firstName, lastName, phoneNumber, email)
        {
        }
    }
}