using Grpc.Core;
using Inventory.Grpc.Repositories.Interfaces;

namespace Inventory.Grpc.Services;
using Inventory.Grpc.Protos;
public class InventoryService : StockProtoService.StockProtoServiceBase
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ILogger _logger;

    public InventoryService(IInventoryRepository inventoryRepository, ILogger logger)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<StockModel> GetStock(GetStockRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"BEGIN Get Stock of ItemNo: {request.ItemNo}");
        var stock = await _inventoryRepository.GetStockAsync(request.ItemNo);
        var result = new StockModel
        {
            Quantity = stock
        };

        _logger.LogInformation($"END Get Stock of ItemNo: {request.ItemNo} - Quantity: {result.Quantity}");
        return result;
    }
}