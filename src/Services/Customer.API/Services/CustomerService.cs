using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IResult> GetCustomerByUserNameAsync(string userName)
        => Results.Ok(await _repository.GetCustomerByUserNameAsync(userName));

    public async Task<IResult> GetCustomersAsync()
        => Results.Ok(await _repository.GetCustomersAsync());

    public async Task<int> CreateCustomerAsync(Entities.Customer customer)
        =>  await _repository.CreateCustomerAsync(customer);

    public async Task UpdateCustomerAsync(Entities.Customer customer)
        => await _repository.UpdateCustomerAsync(customer);

    public async Task DeleteCustomerAsync(int id)
        => await _repository.DeleteCustomerAsync(id);
}