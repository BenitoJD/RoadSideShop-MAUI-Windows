using SQLite;

namespace RoadSideShop.Data
{
    public class MenuCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Icon { get; set; }
    }

}