using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext context, ILogger logger)
    {
        // var domainEntities = context.ChangeTracker.Entries<IEventEntity>()
        //     .Select(x => x.Entity)
        //     .Where(x => x.DomainEvents.Any())
        //
        // await Task.WhenAll(tasks);
    }
}