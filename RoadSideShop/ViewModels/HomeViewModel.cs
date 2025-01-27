using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using RoadSideShop.Data;
using RoadSideShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuItem = RoadSideShop.Data.MenuItem;

namespace RoadSideShop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private MenuCategoryModel[] _categories = [];

        [ObservableProperty]
        private MenuItem[] _menuItems = [];

        [ObservableProperty]
        private MenuCategoryModel? _selectedCategory = null;

        [ObservableProperty]
        private bool _isLoading;

        public ObservableCollection<CartModel> cartItems { get; set; } = new();

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

            MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);

            IsLoading = false;
        }
        [RelayCommand]
        private async Task SelectCategoryAsync(int categoryId)
        {
            if (SelectedCategory?.Id == categoryId)
            {
                return;
            }
            IsLoading = true;

            var existingSelectedCategory = Categories.FirstOrDefault(c => c.IsSelected);
            if (existingSelectedCategory != null)
            {
                existingSelectedCategory.IsSelected = false;
            }

            var newlySelectedCategory = Categories.First(c => c.Id == categoryId);
            newlySelectedCategory.IsSelected = true;

            SelectedCategory = newlySelectedCategory;

            MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);

            IsLoading = false;


        }
        [RelayCommand]
        private void AddtoCart(MenuItem menuItem)
        {
            var cartItem = cartItems.FirstOrDefault(c => c.ItemId == menuItem.Id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartModel
                {
                    ItemId = menuItem.Id,
                    Icon = menuItem.Icon,
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = 1
                });
            }
        }
        [RelayCommand]
        private void IncreaseQuantity(CartModel cartItem)
        {
            cartItem.Quantity++;
        }
        [RelayCommand]
        private void decreaseQuantity(CartModel cartItem)
        {
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                cartItems.Remove(cartItem);
            }
        }
        [RelayCommand]
        private void removeItemFromCart(CartModel cartItem)
        {
            cartItems.Remove(cartItem);
        }

    }
}
   
