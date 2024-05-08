using Customer.API.Services.Interfaces;

namespace Customer.API.Controller;

public static class CustomerController
{
    public static void MapCustomersApi(this WebApplication app)
    {
        app.MapGet("/", () => "Welcome to Customer API!");
        app.MapGet("/api/customers", async (ICustomerService customerService) => await customerService.GetCustomersAsync());
        app.MapGet("/api/customers/{userName}",
            async (ICustomerService customerService, string userName) =>
            {
                var customer = await customerService.GetCustomerByUserNameAsync(userName);
                return customer != null ? Results.Ok(customer) : Results.NotFound();
            });
        app.MapPost("/api/customers",
            async (ICustomerService customerService, Customer.API.Entities.Customer customer) =>
                await customerService.CreateCustomerAsync(customer));
        app.MapPut("/api/customers",
            async (ICustomerService customerService, Customer.API.Entities.Customer customer) =>
                await customerService.UpdateCustomerAsync(customer));
        app.MapDelete("/api/customers/{id}",
            async (ICustomerService customerService, int id) => await customerService.DeleteCustomerAsync(id));
    }
}