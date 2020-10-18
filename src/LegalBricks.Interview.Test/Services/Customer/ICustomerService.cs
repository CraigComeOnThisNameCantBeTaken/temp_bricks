using System.Collections.Generic;
using System.Threading.Tasks;
using DomainModels = LegalBricks.Interview.Test.Models;

namespace LegalBricks.Interview.Test.Services.Customer
{
    public interface ICustomerService
    {
        Task<DomainModels.Customer> CreateAsync(DomainModels.Customer newCustomer);
        Task<DomainModels.Customer> GetByIdAsync(int id);
        Task<IEnumerable<DomainModels.Customer>> GetAsync();
    }
}
