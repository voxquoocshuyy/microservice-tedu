using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrders;
using Ordering.Application.Features.V1.Orders.Commands.DeleteOrders;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrders;
using Ordering.Application.Features.V1.Orders.Queries.GetOrderById;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.DTOs.Order;
using Shared.SeedWork;
using Shared.Services.Email;
using OrderDto = Ordering.Application.Common.Models.OrderDto;

namespace Ordering.API.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public OrdersController(IMediator mediator, ISmtpEmailService smtpEmailService, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    private static class RouteNames
    {
        public const string GetOrders = nameof(GetOrders);
        public const string CreateOrder = nameof(CreateOrder);
        public const string UpdateOrder = nameof(UpdateOrder);
        public const string DeleteOrder = nameof(DeleteOrder);
    }
    [HttpGet("{userName}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string userName)
    {
        var query = new GetOrdersQuery(userName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{userName}", Name = RouteNames.GetOrders)]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersById([Required] long id)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateOrder)]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderDto model)
    {
        var command = _mapper.Map<CreateOrderCommand>(model);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut(Name = RouteNames.UpdateOrder)]
    [ProducesResponseType(typeof(ApiResult<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder([Required]long id, [FromBody] UpdateOrderCommand command)
    {
        command.SetId(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
    [ProducesResponseType(typeof(NoContentResult),StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrder([Required] long id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}