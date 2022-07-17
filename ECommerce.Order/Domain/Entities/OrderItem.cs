﻿namespace ECommerce.Order.Domain.Entities;

public class OrderItem
{
    public OrderItem(string description, decimal price, int quantity)
    {
        Description = description;
        Price = price;
        SetQuantity(quantity);
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public OrderEntity OrderEntity { get; set; }
    public Guid OrderEntityId { get; set; }

    private void SetQuantity(int quantity)
    {
        Quantity = quantity > 0 ? quantity : 1;
    }
}