using CRUDApplication.Business.Abstracts;
using CRUDApplication.Business.Concretes;
using CRUDApplication.Core.Result;
using CRUDApplication.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDto orderDto)
        {
            var result = _orderService.CreateOrder(orderDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result); // Hata mesajı ile birlikte BadRequest döndürülür
            }
            return CreatedAtAction(nameof(GetOrderById), new { id = result.ReturnId }, result); 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var result = _orderService.DeleteOrder(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result); 
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var result = _orderService.GetOrderById(id);
            if (!result.IsSuccess)
            {
                return NotFound(result); 
            }
            return Ok(result); 
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var result = _orderService.GetAllOrders();
            if (!result.IsSuccess)
            {
                return BadRequest(result); 
            }
            return Ok(result); 
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (id != orderDto.Id)
            {
                return BadRequest(new Result().Fail("ID mismatch."));
            }

            var result = _orderService.UpdateOrder(orderDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result); 
            }
            return Ok(result); 
        }
    }
}
