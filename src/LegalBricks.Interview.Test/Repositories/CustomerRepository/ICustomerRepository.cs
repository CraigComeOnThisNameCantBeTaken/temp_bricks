using System.Collections.Generic;
using System.Threading.Tasks;
using LegalBricks.Interview.Test.Models;

namespace LegalBricks.Interview.Test.Repositories.CustomerRepository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAsync();
        Task<Customer> GetAsync(int id);
        Task<Customer> CreateAsync(Customer newCustomer);
    }
}
