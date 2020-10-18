using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LegalBricks.Interview.Test.Exceptions;
using LegalBricks.Interview.Test.Repositories.CustomerRepository;
using LegalBricks.Interview.Test.Services.Customer;
using LegalBrikes.Interview.Test.UnitTests.Mothers;
using Moq;
using Xunit;

namespace LegalBrikes.Interview.Test.UnitTests.Services
{
    // given time constraints I wont decouple concrete here, but in normal case would
    // as I think its unavoidable for low level infrastructure concern tests
    // to be aware of the implementation to assert on eg ISession
    // but for business logic it should not be 1:1
    public class CustomerServiceTests
    {
        private readonly ICustomerService sut;
        private readonly Mock<ICustomerRepository> repoMock;

        public CustomerServiceTests()
        {
            repoMock = new Mock<ICustomerRepository>();
            sut = new CustomerService(repoMock.Object);
        }

        [Fact]
        public async Task CreateCustomer_New_Creates()
        {
            var newCustomer = CustomerMother.Create();
            repoMock.Setup(r => r.CreateAsync(newCustomer))
                .ReturnsAsync(newCustomer);

            var result = await sut.CreateAsync(newCustomer);
            result.Should().BeEquivalentTo(newCustomer);
        }

        [Fact]
        public async Task CreateCustomer_Existing_Creates()
        {
            var newCustomer = CustomerMother.Create();
            repoMock.Setup(r => r.CreateAsync(newCustomer))
                .ThrowsAsync(new DuplicateRecordException());

            await Assert.ThrowsAsync<DuplicateRecordException>(() =>
                sut.CreateAsync(newCustomer));
        }
    }
}
