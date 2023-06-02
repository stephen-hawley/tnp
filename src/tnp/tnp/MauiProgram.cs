using Microsoft.Extensions.Logging;

namespace tnp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<Views.MainPage>();
		builder.Services.AddSingleton<ViewModels.MainPageViewModel>();
		builder.Services.AddTransient<Models.EmptyNodeView>();
		builder.Services.AddTransient<Models.HelloWorldNodeView>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

