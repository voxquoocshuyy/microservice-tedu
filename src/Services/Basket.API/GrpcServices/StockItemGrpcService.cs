using Inventory.Grpc.Protos;

namespace Basket.API.GrpcServices;

public class StockItemGrpcService
{
    private readonly StockProtoService.StockProtoServiceClient _stockServiceClient;

    public StockItemGrpcService(StockProtoService.StockProtoServiceClient stockServiceClient)
    {
        _stockServiceClient = stockServiceClient ?? throw new ArgumentNullException(nameof(stockServiceClient));
    }

    public async Task<StockModel> GetStock(string itemNo)
    {
        try
        {
            var request = new GetStockRequest
            {
                ItemNo = itemNo
            };

            var response = await _stockServiceClient.GetStockAsync(request);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception("Error occurred while getting stock from Inventory gRPC service", e);
        }
    }
}