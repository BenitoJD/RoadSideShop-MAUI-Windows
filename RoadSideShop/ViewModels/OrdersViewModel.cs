using CommunityToolkit.Mvvm.ComponentModel;
using RoadSideShop.Data;
using RoadSideShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadSideShop.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        public async Task PlaceOrderAsync(CartModel[] cartItems, bool isPaidOnline)
        {
            
        }
    }
}
