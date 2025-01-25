using RoadSideShop.Data;

namespace RoadSideShop
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;

        public App(DatabaseService databaseService)
        {
            InitializeComponent();

            if (Current != null)
            {
                Current.UserAppTheme = AppTheme.Light;
            }
            MainPage = new AppShell();
            this._databaseService = databaseService;
        }
        protected override async void OnStart()
        {
            base.OnStart();
            //initialize and seed db
           await _databaseService.InitializeDatabaseAsync();

        }
    }
}
