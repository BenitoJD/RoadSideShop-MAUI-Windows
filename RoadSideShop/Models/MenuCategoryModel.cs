using CommunityToolkit.Mvvm.ComponentModel;
using RoadSideShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadSideShop.Models
{
    public partial class MenuCategoryModel : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        [ObservableProperty]
        private bool _isSelected;

        public static MenuCategoryModel FromEntity(MenuCategory entity)
        {
            return new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Icon = entity.Icon
            };
        }
    }
}
