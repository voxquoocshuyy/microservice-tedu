using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerRepository : RepositoryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
{
    public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext,
        unitOfWork)
    {
    }

    public Task<Entities.Customer> GetCustomerByUserNameAsync(string userName)
        => FindByCondition(c => c.UserName.Equals(userName)).SingleOrDefaultAsync();

    public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync()
        => await FindAll().ToListAsync();

    public async Task<int> CreateCustomerAsync(Entities.Customer customer) => await CreateAsync(customer);
    public async Task UpdateCustomerAsync(Entities.Customer customer) => await UpdateAsync(customer);

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await GetByIdAsync(id);
        if (customer != null) await DeleteAsync(customer);
    }
}