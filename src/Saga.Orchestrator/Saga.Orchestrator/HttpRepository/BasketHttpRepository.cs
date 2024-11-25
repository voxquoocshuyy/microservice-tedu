using Basket.API.Entities;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.HttpRepository;

public class BasketHttpRepository : IBasketHttpRepository
{
    private readonly HttpClient _client;

    public BasketHttpRepository(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task<CartDto> GetBasket(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBasket(string username)
    {
        throw new NotImplementedException();
    }
}