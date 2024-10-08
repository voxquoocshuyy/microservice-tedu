using Infrastructure.Mappings;
using MediatR;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrders;

public class UpdateOrderCommand : IRequest<ApiResult<OrderDto>>
{
    public long Id { get; set; }
    public void SetId(long id)
    {
        Id = id;
    }
    public decimal TotalPrice { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ShippingAddress { get; set; }
    public string InvoiceAddress { get; set; }

    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<UpdateOrderCommand, Order>()
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .IgnoreAllNonExisting();
    }
}