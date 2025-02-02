using RoadSideShop.ViewModels;

namespace RoadSideShop.Pages;

public partial class Orders : ContentPage
{
    private OrdersViewModel _ordersViewModel;

    public Orders(OrdersViewModel ordersViewModel)
	{
		InitializeComponent();
		_ordersViewModel = ordersViewModel;
        InitializeViewModelAsync();
        BindingContext = _ordersViewModel;
    }
    private async void InitializeViewModelAsync()
    {
        
        await _ordersViewModel.InitializeAsync();
    }
}