using SQLite;

namespace RoadSideShop.Data
{
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalItemsCount { get; set; }
        public required string PaymentMode { get; set; } // cash or Online

    }

}