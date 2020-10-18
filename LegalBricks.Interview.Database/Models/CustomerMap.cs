using FluentNHibernate.Mapping;
using LegalBricks.Interview.Database;

public sealed class CustomerMap : ClassMap<Customer>
{
    public CustomerMap()
    {
        Id(x => x.Id).GeneratedBy.Identity();
        Map(x => x.FirstName).Not.Nullable();
        Map(x => x.Surname).Not.Nullable();
        Map(x => x.PhoneNumber).Nullable();
        Map(x => x.Email).Not.Nullable().Unique();
    }
}
