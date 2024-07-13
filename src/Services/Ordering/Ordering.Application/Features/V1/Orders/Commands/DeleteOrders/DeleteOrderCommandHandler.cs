using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrders;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private const string MethodName = nameof(DeleteOrderCommandHandler);

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN: {MethodName} - OrderId: {request.Id}");

        var order = await _orderRepository.GetByIdAsync(request.Id);

        if (order != null)
            await _orderRepository.DeleteAsync(order);

        _logger.LogInformation($"END: {MethodName} - OrderId: {request.Id}");

        return Unit.Value;
    }
}