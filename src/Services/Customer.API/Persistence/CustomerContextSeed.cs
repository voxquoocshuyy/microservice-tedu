using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public static class CustomerContextSeed
{
    public static IHost SeedCustomerData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var customerContext = scope.ServiceProvider.GetRequiredService<CustomerContext>();
        customerContext.Database.MigrateAsync().GetAwaiter().GetResult();
        CreateCustomer(customerContext, "user1", "User", "One", "user1@localhost").GetAwaiter().GetResult();
        CreateCustomer(customerContext, "user2", "User", "Two", "user2@localhost").GetAwaiter().GetResult();

        return host;
    }

    private static async Task CreateCustomer(CustomerContext customerContext, string username, string firstName,
        string lastName, string emailAddress)
    {
        var customer = await customerContext.Customers.SingleOrDefaultAsync(c =>
            c.UserName.Equals(username) || c.EmailAddress.Equals(emailAddress));
        if (customer == null)
        {
            var newCustomer = new Entities.Customer
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress
            };
            await customerContext.Customers.AddAsync(newCustomer);
            await customerContext.SaveChangesAsync();
        }
    }
}