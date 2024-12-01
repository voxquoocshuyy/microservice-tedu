using AutoMapper;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;
using ILogger = Serilog.ILogger;

namespace Saga.Orchestrator.Services;

public class CheckoutSagaService : ICheckoutSagaService
{
    private readonly IOrderHttpRepository _orderHttpRepository;
    private readonly IBasketHttpRepository _basketHttpRepository;
    private readonly IInventoryHttpRepository _inventoryHttpRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckoutSagaService(IInventoryHttpRepository inventoryHttpRepository, IBasketHttpRepository basketHttpRepository,
        IOrderHttpRepository orderHttpRepository, IMapper mapper, ILogger logger)
    {
        _inventoryHttpRepository = inventoryHttpRepository;
        _basketHttpRepository = basketHttpRepository;
        _orderHttpRepository = orderHttpRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<bool> CheckoutOrder(string username, BasketCheckoutDto basketCheckout)
    {
        //Get cart from basket http repository
        _logger.Information($"START: Get cart {username}");
        var cart = await _basketHttpRepository.GetBasket(username);
        if (cart == null) return false;
        _logger.Information($"END: Get cart {username} successfully");

        //Create order
        _logger.Information($"START: Create order {username}");
        var order = _mapper.Map<CreateOrderDto>(basketCheckout);
        order.TotalPrice = cart.TotalPrice;
        // Get order by order id
        var orderId = await _orderHttpRepository.CreateOrder(order);
        if (orderId < 0) return false;
        var addedOrder = await _orderHttpRepository.GetOrder(orderId);
        _logger.Information($"END: Create order: {orderId} - Document No: {addedOrder.DocumentNo} successfully");
        // Sales Items
        var inventoryDocumentNos = new List<string>();
        bool result;
        try
        {
            foreach (var item in cart.Items)
            {
                _logger.Information($"START: Sale item no: {item.ItemNo} - Quantity: {item.Quantity}");
                var salesProduct = new SalesProductDto(addedOrder.DocumentNo, item.Quantity);
                salesProduct.SetItemNo(item.ItemNo);
                var documentNo = await _inventoryHttpRepository.CreateSalesOrder(salesProduct);
                inventoryDocumentNos.Add(documentNo);
                _logger.Information(
                    $"END: Sale item no: {item.ItemNo} - Quantity: {item.Quantity} - Document No: {documentNo} successfully");
            }
            // Delete basket
            _logger.Information($"START: Delete basket {username}");
            result = await _basketHttpRepository.DeleteBasket(username);
            _logger.Information($"END: Delete basket {username} successfully");
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error occurred while creating sales order");
            await RollbackCheckoutOrder(username, orderId, inventoryDocumentNos);
            return false;
        }

        return result;
    }

    private async Task RollbackCheckoutOrder(string username, long orderId, List<string> inventoryDocumentNos)
    {
        _logger.Information($"START: RollbackCheckoutOrder for username: {username}, " +
                           $"orderId: {orderId}, inventoryDocumentNos: {string.Join(",", inventoryDocumentNos)}");
        var deletedListDocumentNos = new List<string>();
        // Delete order by order id
        _logger.Information($"START: Delete order by order id: {orderId}");
        foreach (var documentNo in inventoryDocumentNos)
        {
            await _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
            deletedListDocumentNos.Add(documentNo);
        }
        _logger.Information($"END: Deleted inventory document nos: {string.Join(",", deletedListDocumentNos)} successfully");
    }
}