using Shared.Enums.Inventory;

namespace Shared.DTOs.Inventory;

public record SalesProductDto(string ExternalDocumentNo, int Quantity)
{
    public EDocumentType DocumentType = EDocumentType.Sale;
    public string ItemNo { get; private set; }

    public void SetItemNo(string itemNo)
    {
        ItemNo = itemNo;
    }
}