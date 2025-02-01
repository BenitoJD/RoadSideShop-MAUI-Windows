using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using RoadSideShop.Data;
using RoadSideShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuItem = RoadSideShop.Data.MenuItem;

namespace RoadSideShop.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty,NotifyPropertyChangedFor(nameof(TaxAmount))]
        [NotifyPropertyChangedFor(nameof(Total))]
        private decimal _subtotal;

        
        [ObservableProperty, NotifyPropertyChangedFor(nameof(TaxAmount))]
        [NotifyPropertyChangedFor(nameof(Total))]
        private int _taxPercentage;

        public decimal TaxAmount => (Subtotal * TaxPercentage) / 100;

        public decimal Total => Subtotal + TaxAmount;

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
            cartItems.CollectionChanged += CartItems_CollectionChanged;
        }

        private void CartItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RecalculateAmounts();
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
                RecalculateAmounts();

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
            RecalculateAmounts();
        }
        [RelayCommand]
        private void decreaseQuantity(CartModel cartItem)
        {
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                RecalculateAmounts();

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

        private void RecalculateAmounts()
        {
            Subtotal = cartItems.Sum(c => c.Amount);
        }
        [RelayCommand]
        private async Task TaxPercentageClickAsync()
        {
            var result = await Shell.Current.DisplayPromptAsync("Tax Percentage", "Enter the applicable tax percentage", placeholder: "10", initialValue: TaxPercentage.ToString());

            if (result != null && int.TryParse(result, out var taxPercentage))
            {
                if (taxPercentage > 100)
                {
                    await Shell.Current.DisplayAlert("Tax Percentage", "Tax percentage cannot be more than 100%", "Ok");
                    return;
                }
                if (taxPercentage < 0)
                {
                    await Shell.Current.DisplayAlert("Tax Percentage", "Tax percentage cannot be less than 0%", "Ok");
                    return;
                }
                await Shell.Current.DisplayAlert("Tax Percentage", $"Tax percentage updated to {taxPercentage}%", "Ok");
                TaxPercentage = taxPercentage;
            }
            else
            {
                await Shell.Current.DisplayAlert("Tax Percentage", "Invalid tax percentage", "Ok");
            }
        }
    }
}
   
