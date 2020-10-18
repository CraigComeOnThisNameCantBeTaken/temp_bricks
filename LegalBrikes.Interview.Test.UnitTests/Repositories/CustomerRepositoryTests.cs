using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LegalBricks.Interview.Database;
using LegalBricks.Interview.Test.Exceptions;
using LegalBricks.Interview.Test.Repositories.CustomerRepository;
using LegalBrikes.Interview.Test.UnitTests.Mothers;
using Xunit;
using DatabaseModels = LegalBricks.Interview.Database;
using DomainModels = LegalBricks.Interview.Test.Models;

namespace LegalBrikes.Interview.Test.UnitTests.Repositories
{
    public class CustomerRepositoryTests
    {
        private readonly NHibernateRepositoryFactory repoFactory;

        public CustomerRepositoryTests()
        {
            repoFactory = new NHibernateRepositoryFactory();
        }

        [Fact]
        public async Task CreateAsync_RecordDoesNotExist_CreatesRecord()
        {
            var newCustomer = CustomerMother.Create();
            var sut = Create();
            await sut.CreateAsync(newCustomer);

            var inserted = repoFactory.Session.Query<DatabaseModels.Customer>()
                .FirstOrDefault();

            inserted.Should().BeEquivalentTo(newCustomer);
        }

        [Fact]
        public async Task CreateAsync_RecordDoesExistWithSameEmail_ThrowsException()
        {
            var newCustomer = CustomerMother.Create();

            // as in the actual repo this should be transparent but due to time constraints being done here
            var dbEntity = new DatabaseModels.Customer
            {
                Id = newCustomer.Id,
                FirstName = newCustomer.FirstName,
                Surname = newCustomer.Surname,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email
            };
            dbEntity = repoFactory.Add(dbEntity);

            var duplicateCustomer = new DomainModels.Customer
            {
                Id = dbEntity.Id,
                FirstName = "new",
                Surname = "new",
                PhoneNumber = "949383840",
                Email = "new@new.com"
            };

            var sut = Create();
            // a more specific exception in real life project
            await Assert.ThrowsAsync<DuplicateRecordException>(() => sut.CreateAsync(duplicateCustomer));
        }

        [Fact]
        public async Task CreateAsync_RecordDoesExistWithSameId_ThrowsException()
        {
            var newCustomer = CustomerMother.Create();

            // as in the actual repo this should be transparent but due to time constraints being done here
            var dbEntity = new DatabaseModels.Customer
            {
                Id = newCustomer.Id,
                FirstName = newCustomer.FirstName,
                Surname = newCustomer.Surname,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email
            };
            dbEntity = repoFactory.Add(dbEntity);

            var duplicateCustomer = new DomainModels.Customer
            {
                Id = 2,
                FirstName = "new",
                Surname = "new",
                PhoneNumber = "949383840",
                Email = dbEntity.Email
            };

            var sut = Create();
            // a more specific exception in real life project
            await Assert.ThrowsAnyAsync<DuplicateRecordException>(() => sut.CreateAsync(duplicateCustomer));
        }

        [Fact]
        public async Task GetAsync_CustomerExist_ReturnsAllCustomers()
        {
            var existing = new List<DomainModels.Customer> {
                CustomerMother.Create("emailOne@email.com"),
                CustomerMother.Create("emailTwo@email.com")
            };

            foreach (var item in existing)
            {
                var dbEntity = new DatabaseModels.Customer
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    Surname = item.Surname,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email
                };
                repoFactory.Add(dbEntity);
                item.Id = dbEntity.Id;
            }

            var sut = Create();
            var result = await sut.GetAsync();
            result.Should().BeEquivalentTo(existing);
        }

        [Fact]
        public async Task GetAsync_CustomerDontExist_ReturnsEmptyList()
        {
            var sut = Create();
            var result = await sut.GetAsync();
            result.Should().BeEmpty();
        }


        [Fact]
        public async Task GetAsyncById_CustomerDoesExist_ReturnsCustomer()
        {
            var newCustomer = CustomerMother.Create();

            // as in the actual repo this should be transparent but due to time constraints being done here
            var dbEntity = new DatabaseModels.Customer
            {
                Id = newCustomer.Id,
                FirstName = newCustomer.FirstName,
                Surname = newCustomer.Surname,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email
            };
            dbEntity = repoFactory.Add(dbEntity);

            var sut = Create();
            var result = await sut.GetAsync(dbEntity.Id);
            result.Should().BeEquivalentTo(dbEntity);
        }

        [Fact]
        public async Task GetAsyncById_CustomerDoesntExist_ReturnsNull()
        {
            var sut = Create();
            var result = await sut.GetAsync(1);
            result.Should().BeNull();
        }

        private ICustomerRepository Create()
        {
            return repoFactory.Create<NHibernateCustomerRepository>();
        }
    }
}
