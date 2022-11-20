using Microsoft.Extensions.DependencyInjection;
using timeboxed.Pages;
using Uno.Extensions.Logging;
using Windows.ApplicationModel.Activation;

namespace timeboxed
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private Window _window;
        public IServiceProvider Container { get; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {

            this.InitializeComponent();

#if HAS_UNO || NETFX_CORE
            this.Suspending += OnSuspending;
#endif
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
            _window = new Window();
            _window.Activate();
#else
            _window = Microsoft.UI.Xaml.Window.Current;
#endif

            _window.AttachNavigation(Host.Services,initialRoute:"");


            _window.Activate();

            await Task.Run(() => Host.StartAsync());

            var logger = Host.Services.GetRequiredService<ILogger<App>>();
            if (logger.IsEnabled(LogLevel.Trace)) logger.LogTraceMessage("LogLevel:Trace");
            if (logger.IsEnabled(LogLevel.Debug)) logger.LogDebugMessage("LogLevel:Debug");
            if (logger.IsEnabled(LogLevel.Information)) logger.LogInformationMessage("LogLevel:Information");
            if (logger.IsEnabled(LogLevel.Warning)) logger.LogWarningMessage("LogLevel:Warning");
            if (logger.IsEnabled(LogLevel.Error)) logger.LogErrorMessage("LogLevel:Error");
            if (logger.IsEnabled(LogLevel.Critical)) logger.LogCriticalMessage("LogLevel:Critical");
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
