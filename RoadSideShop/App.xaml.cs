using RoadSideShop.Data;

namespace RoadSideShop
{
    public partial class App : Application
    {

        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            if (Current != null)
            {
                Current.UserAppTheme = AppTheme.Light;
            }
            MainPage = new AppShell();
            Task.Run(async() => await databaseService.InitializeDatabaseAsync()).GetAwaiter().GetResult();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window =  base.CreateWindow(activationState); 
            
            window.MinimumHeight = 760;
            window.MinimumWidth = 1280;
            return window;
        }

    }
}
