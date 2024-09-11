using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrders;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<long>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public CreateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private const string MethodName = nameof(CreateOrderCommandHandler);

    public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN: {MethodName} - UserName: {request.UserName}");

        var orderEntity = _mapper.Map<Order>(request);
        _orderRepository.CreateOrder(orderEntity);
        orderEntity.AddedOrder();
        await _orderRepository.SaveChangesAsync();

        _logger.LogInformation($"Order {orderEntity.Id} is created successfully.");

        _logger.LogInformation($"END: {MethodName} - UserName: {request.UserName}");

        return new ApiSuccessResult<long>(orderEntity.Id);
    }
}