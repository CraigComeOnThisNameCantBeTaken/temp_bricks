using LegalBricks.Interview.Test.Models;

namespace LegalBrikes.Interview.Test.UnitTests.Mothers
{
    public static class CustomerMother
    {
        public static Customer Create()
        {
            return new Customer
            {
                Id = default,
                FirstName = "aFirstName",
                Surname = "aSurname",
                PhoneNumber = "0148560982",
                Email = "anEmail@anEmailThatIsValid.com"
            };
        }

        public static Customer Create(string email)
        {
            return new Customer
            {
                Id = default,
                FirstName = "aFirstName",
                Surname = "aSurname",
                PhoneNumber = "0148560982",
                Email = email
            };
        }
    }
}
