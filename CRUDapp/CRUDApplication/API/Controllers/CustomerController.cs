using CRUDApplication.Business.Abstracts;
using CRUDApplication.Business.Concretes;
using CRUDApplication.Core.Result;
using CRUDApplication.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var result = _customerService.GetAllCustomers();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var result = _customerService.GetCustomerById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var result = _customerService.CreateCustomer(customerDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return CreatedAtAction(nameof(GetCustomerById), new { id = result.ReturnId }, result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (id != customerDto.id)
            {
                return BadRequest(new Result().Fail("ID mismatch."));
            }

            var result = _customerService.UpdateCustomer(customerDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var result = _customerService.DeleteCustomer(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
