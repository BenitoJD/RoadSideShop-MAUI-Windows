﻿using SQLite;

namespace RoadSideShop.Data
{
    public class OrderItem
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int ItemId { get; set; }
        public required string Name { get; set; }
        public required string Icon { get;set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [Ignore]
        public decimal Amount => Quantity * Price;
    }

}