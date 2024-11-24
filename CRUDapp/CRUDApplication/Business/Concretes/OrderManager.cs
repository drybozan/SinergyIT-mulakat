using AutoMapper;
using Castle.Core.Resource;
using CRUDApplication.Business.Abstracts;
using CRUDApplication.Core.Result;
using CRUDApplication.Data.Repositories.Abstracts;
using CRUDApplication.Data.Repositories.Concretes;
using CRUDApplication.Entities;
using CRUDApplication.Entities.DTOs;

namespace CRUDApplication.Business.Concretes
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderManager(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }


        public Result CreateOrder(OrderDto orderDto)
        {
            try
            {
                var order = _mapper.Map<Order>(orderDto);
                _orderRepository.Add(order);
                return new Result().Success("Order created successfully.", order.id);
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while creating the order.", ex.Message);
            }
        }

        public Result DeleteOrder(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return new Result().Fail("Order not found.");
            }

            try
            {
                _orderRepository.Delete(order);  // Deleting the order by ID
                return new Result().Success("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while deleting the order.", ex.Message);
            }
        }

        public Result GetOrderById(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order == null)
            {
                return new Result().Fail("Order not found.");
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return new Result().Success("Order retrieved successfully", orderDto);
        }

        public Result GetAllOrders()
        {

           
            try
            {
                var orders = _orderRepository.GetAll();

                if (orders == null || !orders.Any())
                {
                    return new Result { IsSuccess = false, Message = "An error occurred while retrieving orders.", data = null };
                }
                var orderDtos = _mapper.Map<List<OrderDto>>(orders);
                return new Result { IsSuccess = true, Message = "Orders retrieved successfully", data = orderDtos };
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while retrieving orders.", ex.Message);
            }
        }

        public Result UpdateOrder(OrderDto orderDto)
        {
            var order = _orderRepository.GetById(orderDto.Id);
            if (order == null)
            {
                return new Result().Fail("Order not found.");
            }

            try
            {
                _mapper.Map(orderDto, order);
                _orderRepository.Update(order);
                return new Result().Success("Order updated successfully.", order.id);
            }
            catch (Exception ex)
            {
                return new Result().Fail("An error occurred while updating the order.", ex.Message);
            }
        }
    }
}
