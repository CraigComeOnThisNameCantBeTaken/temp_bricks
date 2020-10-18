using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LegalBricks.Interview.Test.Exceptions;
using LegalBricks.Interview.Test.Models;
using LegalBricks.Interview.Test.Services.Customer;
using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace LegalBricks.Interview.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _customerService.GetAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([Required] Customer newCustomer)
        {
            try
            {
                var result = await _customerService.CreateAsync(newCustomer);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch(DuplicateRecordException e)
            {
                return BadRequest("Record already exists");
            }
        }
    }
}
