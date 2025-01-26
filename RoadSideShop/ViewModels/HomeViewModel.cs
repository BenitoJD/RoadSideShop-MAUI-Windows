using CommunityToolkit.Mvvm.ComponentModel;
using RoadSideShop.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadSideShop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        public MenuCategory[] _categories = [];

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        private bool _isInitialized;
        public async ValueTask InitializeAsync()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;
            Categories = await _databaseService.GetMenuCategoriesAsync();
        }
    }
}
