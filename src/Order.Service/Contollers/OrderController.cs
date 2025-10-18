using common.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Service.Features.Orders.Commands;
using Order.Service.Features.Orders.Dtos;
using Order.Service.Features.Orders.Queries;

namespace Order.Service.Contollers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }


        ///// <summary>
        ///// Get all orders
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[ProducesResponseType(typeof(SuccessResponseDto<PagedListDto<OrderHeaderResponseDto>>), StatusCodes.Status200OK)]
        //public async Task<IActionResult> GetAllOrders()
        //{
        //    var query = new GetAllOrdersQuery();

        //    var result = await _mediator.Send(query);

        //    var response = new SuccessResponseDto<PagedListDto<OrderHeaderResponseDto>>
        //    {
        //        Data = result,
        //        Message = "Orders retrieved successfully"
        //    };

        //    return Ok(response);
        //}


        /// <summary>
        /// Add Order - add a order 
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>> CreateOrder([FromBody] OrderHeaderCreateDto body)
        {
            var command = new CreateOrderCommand(body);
            var response = await _mediator.Send(command);
            if (response.Data == null)
            {
                return BadRequest(response);
            }
            return Created("", response);
        }


        /// <summary>
        /// Get order by order Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderSuccessAndErrorResponseDto<OrderHeaderResponseDto>>> GetOrderById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { error = "Invalid order ID" });
            }

            var query = new GetOrderByIdQuery(id);
            var order = await _mediator.Send(query);

            return Ok(order);
        }
    }
}
