using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrders;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrders;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private const string MethodName = nameof(UpdateOrderCommandHandler);

    public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
        if (orderEntity == null) throw new NotFoundException(nameof(Order), request.Id);

        _logger.LogInformation($"BEGIN: {MethodName} - UserName: {request.FirstName}");

        orderEntity = _mapper.Map(request, orderEntity);
        var updatedOrder = await _orderRepository.UpdateOrderAsync(orderEntity);
        await _orderRepository.SaveChangesAsync();
        _logger.LogInformation($"Order {updatedOrder.Id} is updated successfully.");
        var result = _mapper.Map<OrderDto>(updatedOrder);

        _logger.LogInformation($"END: {MethodName} - UserName: {request.FirstName}");

        return new ApiSuccessResult<OrderDto>(result);
    }
}