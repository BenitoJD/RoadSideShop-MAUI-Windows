using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoadSideShop.Data;
using RoadSideShop.Models;
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
        private MenuCategoryModel[] _categories = [];
        [ObservableProperty]
        private MenuCategoryModel? _selectedCategory = null;

        [ObservableProperty]
        private bool _isLoading;

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

            IsLoading = true;

            Categories = (await _databaseService.GetMenuCategoriesAsync())
                .Select(MenuCategoryModel.FromEntity)
                .ToArray();
            Categories[0].IsSelected = true;

            SelectedCategory = Categories[0];

            IsLoading = false;
        }
        [RelayCommand]
        private void SelectCategory(int categoryId)
        {
            if (SelectedCategory?.Id == categoryId)
            {
                return;
            }

            var existingSelectedCategory = Categories.FirstOrDefault(c => c.IsSelected);
            if (existingSelectedCategory != null)
            {
                existingSelectedCategory.IsSelected = false;
            }

            var newlySelectedCategory = Categories.First(c => c.Id == categoryId);
            newlySelectedCategory.IsSelected = true;

            SelectedCategory = newlySelectedCategory;
        }

    }
}
   
