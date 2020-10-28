using System;
using System.Collections.Generic;
using Core.Entities.OrderAggregate;

namespace API.Dto
{
    public class OrderToReturnDto
    {
        public int id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal total { get; set; }
        public string Status { get; set; }
    }
}