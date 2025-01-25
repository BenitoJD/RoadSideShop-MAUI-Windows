using SQLite;

namespace RoadSideShop.Data
{
    public class MenuItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required string Icon { get; set; }
    }

}