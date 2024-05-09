using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerContext>
{
    Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName);
    Task<IEnumerable<Entities.Customer>> GetCustomersAsync();
    Task<int> CreateCustomerAsync(Entities.Customer customer);
    Task UpdateCustomerAsync(Entities.Customer customer);
    Task DeleteCustomerAsync(int id);
}