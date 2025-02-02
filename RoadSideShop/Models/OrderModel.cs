using RoadSideShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadSideShop.Models
{
    public class OrderModel
    {
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItemsCount { get; set; }
        public string PaymentMode { get; set; } // cash or Online

        public OrderItem[] Items { get; set; } = Array.Empty<OrderItem>();
    }
}
