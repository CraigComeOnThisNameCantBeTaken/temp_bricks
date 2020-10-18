using System.Collections.Generic;
using System.Threading.Tasks;
using LegalBricks.Interview.Test.Repositories.CustomerRepository;

namespace LegalBricks.Interview.Test.Services.Customer
{
    // this layer exists purely to decouple db logic from business
    // but as this is quite a small problem to solve it may not be obvious why

    // assuming the project grew we might want greater business logic such as sending an email
    // updating a second API, firing an event. As it is now it is just a thin
    // wrapper that ensures future changes / follows a consistent pattern I would use to limit change
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public Task<Models.Customer> CreateAsync(Models.Customer newCustomer)
        {
            return _customerRepo.CreateAsync(newCustomer);
        }

        public Task<IEnumerable<Models.Customer>> GetAsync()
        {
            return _customerRepo.GetAsync();
        }

        public Task<Models.Customer> GetByIdAsync(int id)
        {
            return _customerRepo.GetAsync(id);
        }
    }
}
