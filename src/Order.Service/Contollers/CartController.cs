using common.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Service.Features.CartItems.Commands;
using Order.Service.Features.CartItems.Dtos;
using Order.Service.Features.CartItems.Queries;

namespace Order.Service.Contollers
{
    [Route("api/v1/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private const string AdminRole = "Admin";
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add a product to the cart
        /// </summary>
        [HttpPost]
        [Authorize(Roles = AdminRole)]
        [ProducesResponseType(typeof(CartErrorAndSuccessResponseDto<CartResponseDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<CartErrorAndSuccessResponseDto<CartResponseDto>>> AddToCart([FromBody] CartItemDto body)
        {
            var command = new AddToCartCommand(body);
            var response = await _mediator.Send(command);
            if (response.Data == null)
            {
                return BadRequest(response);
            }

            return Created("", response);
        }

        /// <summary>
        /// Get all items in the current user's cart
        /// </summary>
        [HttpGet()]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponseDto<IEnumerable<CartDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<SuccessResponseDto<IEnumerable<CartDto>>>> GetCart()
        {
            var result = await _mediator.Send(new GetCartQueryById());
            var response = new SuccessResponseDto<IEnumerable<CartDto>>
            {
                Data = result,
                Message = "Cart items retrieved successfully"
            };

            return Ok(response);
        }

        /// <summary>
        /// Delete a specific product from the cart
        /// </summary>
        [HttpDelete("{cartId}")]
        [ProducesResponseType(typeof(CartErrorAndSuccessResponseDto<CartResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CartErrorAndSuccessResponseDto<CartResponseDto>>> DeleteFromCart(Guid cartId)
        {
            if (cartId == Guid.Empty)
            {
                return BadRequest(new { error = "Invalid cartId ID" });
            }

            var command = new DeleteFromCartCommand(cartId);
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
