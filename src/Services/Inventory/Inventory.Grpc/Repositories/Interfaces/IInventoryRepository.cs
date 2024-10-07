using Contracts.Domains.Interfaces;
using Inventory.Grpc.Entities;

namespace Inventory.Grpc.Repositories.Interfaces;

public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
{
    Task<int> GetStockAsync(string itemNo);
}