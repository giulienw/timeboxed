
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using timeboxed.Models;
using timeboxed.Pages;
using timeboxed.Services;
using timeboxed.ViewModels;
using Uno.Extensions;
using Uno.Extensions.Configuration;
using Uno.Extensions.Hosting;
using Uno.Extensions.Navigation;

namespace timeboxed;

public sealed partial class App : Application
{
    public IHost Host { get; } = BuildAppHost();

    private static IHost BuildAppHost()
    {
        return UnoHost
                .CreateDefaultBuilder()
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                // Add platform specific log providers
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                            .SetMinimumLevel(
                                context.HostingEnvironment.IsDevelopment() ?
                                    LogLevel.Information :
                                    LogLevel.Warning);
                })

				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<App>()
				)

                // Register services for the application
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IOpenSenseClient, OpenSenseClient>();
                    services.AddSingleton<DatabaseClient>();
                    services.AddTransient<ListPageViewModel>();
                    services.AddTransient<BoxInfoPageViewModel>();
                    services.AddSingleton<NavPage>();
                    services.AddNativeHandler();
                })

                // Enable navigation, including registering views and viewmodels
				.UseNavigation(RegisterRoutes)

                // Add navigation support for toolkit controls such as TabBar and NavigationView
                .UseToolkitNavigation()

                .Build(enableUnoLogging: true);

    }
    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap<BoxInfoPage, BoxInfoPageViewModel>(Data: new DataMap<BoxData>()),
            new ViewMap<ShellControl, ShellViewModel>(),
            new ViewMap<MainPage,ListPageViewModel>(),
            new ViewMap<NavPage>()
            );

        routes
            .Register(
                new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                        Nested: new RouteMap[]
                        {
                            new RouteMap("home", View: views.FindByView<NavPage>()),
                            new RouteMap("list", View: views.FindByView<MainPage>()),
                            new RouteMap("info",View: views.FindByViewModel<BoxInfoPageViewModel>()),
                        }));
    }
}
