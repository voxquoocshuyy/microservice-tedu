using System.ComponentModel.DataAnnotations;
using System.Net;
using Infrastructure.Common.Models;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;

namespace Inventory.Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    /// <summary>
    /// api/inventory/items/{itemNo}
    /// </summary>
    /// <returns></returns>
    [Route("items/{itemNo}", Name = "GetAllByItemNo")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNoAsync([Required] string itemNo)
    {
        var result = await _inventoryService.GetAllByItemNoAsync(itemNo);
        return Ok(result);
    }

    /// <summary>
    /// api/inventory/items/{itemNo}/paging
    /// </summary>
    /// <returns></returns>
    [Route("items/{itemNo}/paging", Name = "GetAllByItemNoPaging")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNoPagingAsync([Required] string itemNo,
        [FromQuery] GetInventoryPagingQuery query)
    {
        query.SetItemNo(itemNo);
        var result = await _inventoryService.GetAllByItemNoPagingAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// api/inventory/{id}
    /// </summary>
    /// <returns></returns>
    [Route("{id}", Name = "GetById")]
    [HttpGet]
    [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> GetByIdAsync([Required] string id)
    {
        var result = await _inventoryService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// api/inventory/purchase/{itemNo}
    /// </summary>
    /// <return></return>
    [Route("purchase/{itemNo}", Name = "PurchaseOrder")]
    [HttpPost]
    public async Task<ActionResult<InventoryEntryDto>> PurchaseOrderAsync([Required] string itemNo, [FromBody] PurchaseProductDto model)
    {
        var result = await _inventoryService.PurchaseItemAsync(itemNo, model);
        return Ok(result);
    }

    /// <summary>
    /// api/inventory/{id}
    /// </summary>
    /// <returns></returns>
    [Route("{id}", Name = "DeleteById")]
    [HttpDelete]
    public async Task<ActionResult> DeleteByIdAsync([Required] string id)
    {
        var entity = await _inventoryService.GetByIdAsync(id);
        if (entity == null) return NotFound();

        await _inventoryService.DeleteAsync(id);
        return NoContent();
    }
}