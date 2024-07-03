using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger;
    }

    private const string MethodName = nameof(GetOrdersQueryHandler);

    public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN: {MethodName} - UserName: {request.UserName}");

        var ordersEntities = await _orderRepository.GetOrdersByUserName(request.UserName);
        var orderList = _mapper.Map<List<OrderDto>>(ordersEntities);

        _logger.LogInformation($"END: {MethodName} - UserName: {request.UserName}");

        return new ApiSuccessResult<List<OrderDto>>(orderList);
    }
}