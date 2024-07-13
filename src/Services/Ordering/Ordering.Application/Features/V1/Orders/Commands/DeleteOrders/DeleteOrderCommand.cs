using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrders;

public class DeleteOrderCommand : IRequest
{
    public long Id { get; private set; }
    
    public DeleteOrderCommand(long id)
    {
        Id = id;
    }
}