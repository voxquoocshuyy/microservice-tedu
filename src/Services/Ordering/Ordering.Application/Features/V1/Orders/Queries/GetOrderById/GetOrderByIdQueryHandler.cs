using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Serilog;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger;
    }

    private const string MethodName = nameof(GetOrderByIdQueryHandler);

    public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        // _logger.LogInformation($"BEGIN: {MethodName} - UserName: {request.Id}");

        var order = await _orderRepository.GetByIdAsync(request.Id);
        var orderDto = _mapper.Map<OrderDto>(order);

        // _logger.LogInformation($"END: {MethodName} - UserName: {request.Id}");

        return new ApiSuccessResult<OrderDto>(orderDto);
    }
}