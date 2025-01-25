using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace RoadSideShop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                
                    fonts.AddFont("Poppins-Regular.ttf", "Poppins-Regular");
                    fonts.AddFont("Poppins-Bold.ttf", "Poppins-Bold");

                })
                .UseMauiCommunityToolkit();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
