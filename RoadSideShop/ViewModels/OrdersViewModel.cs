using CommunityToolkit.Maui.Alerts;
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
    public partial class OrdersViewModel : ObservableObject
    {
        private readonly DatabaseService databaseService;
        [ObservableProperty]
        private OrderItem[] _orderItems = [];


        public OrdersViewModel(DatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public ObservableCollection<Order> Orders { get; set; } = [];
        public async Task<bool> PlaceOrderAsync(CartModel[] cartItems, bool isPaidOnline)
        {
            var orderItems = cartItems.Select(x => new OrderItem
            {
                Icon = x.Icon,
                ItemId = x.ItemId,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
            });
            var orderModel = new OrderModel
            {
                OrderDate = DateTime.Now,
                PaymentMode = isPaidOnline ? "Online" : "Cash",
                TotalAmount = cartItems.Sum(x => x.Amount),
                TotalItemsCount = cartItems.Length,
                Items = orderItems.ToArray(),
            };
            var errorMessage = await databaseService.PlaceOrderAsync(orderModel);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                await Shell.Current.DisplayAlert("Error", errorMessage, "Ok");
                return false;
            }
            await Toast.Make("Order Placed Successfully").Show();
            return true;

        }

        private bool _isinitialized;

        [ObservableProperty]
        private bool _isLoading;
        public async Task InitializeAsync()
        {
            if(_isinitialized)
            {
                return;
            }
            _isinitialized = true;
            IsLoading = true;
            var orders = await databaseService.GetOrdersAsync();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            IsLoading = false;
        }

        [RelayCommand]
        public async Task SelectOrderAsync(Order? order)
        {
            if(order == null || order.Id == 0)
            {
                OrderItems = [];
                return;

            }
            IsLoading = true;

            OrderItems = await databaseService.GetOrderItemsAsync(order.Id);
            IsLoading = false;
        }
        
    }
}
