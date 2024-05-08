namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<IResult> GetCustomerByUserNameAsync(string userName);
    Task<IResult> GetCustomersAsync();
    Task<int> CreateCustomerAsync(Entities.Customer customer);
    Task UpdateCustomerAsync(Entities.Customer customer);
    Task DeleteCustomerAsync(int id);
}