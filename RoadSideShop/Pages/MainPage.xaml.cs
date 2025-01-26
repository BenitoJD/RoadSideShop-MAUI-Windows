using RoadSideShop.ViewModels;

namespace RoadSideShop.Pages
{
    public partial class MainPage : ContentPage
    {
                private readonly HomeViewModel _homeViewModel;
        public MainPage(HomeViewModel homeViewModel)
        {
            InitializeComponent();
            _homeViewModel = homeViewModel;
            BindingContext = _homeViewModel;
            Initialize();
        }

        private async void Initialize()
        {
            await _homeViewModel.InitializeAsync();
        }
        



    }

}
