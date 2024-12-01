using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Order;

namespace Saga.Orchestrator.HttpRepository;

public class OrderHttpRepository : IOrderHttpRepository
{
    public Task<long> CreateOrder(CreateOrderDto order)
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> GetOrder(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOrder(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOrderByDocumentNo(string documentNo)
    {
        throw new NotImplementedException();
    }
}