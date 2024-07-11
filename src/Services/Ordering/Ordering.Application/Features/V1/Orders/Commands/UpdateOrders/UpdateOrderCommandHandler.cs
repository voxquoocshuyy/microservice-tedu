using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        _logger.LogInformation($"BEGIN: {MethodName} - UserName: {request.UserName}");

        var orderEntity = _mapper.Map<Order>(request);
        var newOrder = _orderRepository.UpdateAsync(orderEntity);
        var result = _mapper.Map<OrderDto>(newOrder);

        _logger.LogInformation($"END: {MethodName} - UserName: {request.UserName}");

        return new ApiSuccessResult<OrderDto>(result);
    }
}