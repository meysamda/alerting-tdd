namespace Alerting.Application.Common.DomainExceptions
{
    public enum ErrorMessage
    {
        // bad request (400 series) client error details
        invalidEmail,
        entityWithTheSameKeyAlreadyExists,

        // unauthorized (401 series) client error details
        tokenNotVerified,

        // forbidden (403 series) client error details
        cantSignIn,

        // not found (404 series) client error details
        contactPersonNotFound,

        // conflict (409 series) client error details
        alreadyConfirmed,
    }
}