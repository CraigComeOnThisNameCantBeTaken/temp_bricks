namespace LegalBricks.Interview.Database
{ 
    public class Customer : IEntity
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string Surname { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual string Email { get; set; }
    }
}
