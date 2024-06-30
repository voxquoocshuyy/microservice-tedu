using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence;

public class OrderContextSeed
{
    private readonly ILogger _logger;
    private readonly OrderContext _context;

    public OrderContextSeed(ILogger logger, OrderContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
                await _context.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.Error("An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.Error("An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(
                    new Order
                    {
                        UserName = "Customer1",
                        FirstName = "Customer1",
                        LastName = "Customer",
                        EmailAddress = "Customer@local.com",
                        ShippingAddress = "123 Main St",
                        InvoiceAddress = "123 Main St",
                        TotalPrice = 250
                    });
            }
        }
        catch (Exception e)
        {
            _logger.Error("An error occurred while seeding the database.");
            throw;
        }
    }
}