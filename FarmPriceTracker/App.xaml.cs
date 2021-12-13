using System.Windows;
using AssemblyScanning;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace FarmPriceTracker;

/// <summary>
///   Interaction logic for App.xaml
/// </summary>
public partial class App {
  private readonly ServiceProvider _serviceProvider;

  public App() {
    _serviceProvider = InitializeSplat();
  }

  private static ServiceProvider InitializeSplat() {
    var services = new ServiceCollection();

    services.UseMicrosoftDependencyResolver();
    IMutableDependencyResolver resolver = Locator.CurrentMutable;

    resolver.InitializeSplat();
    resolver.InitializeReactiveUI();

    ConfigureServices(services);

    Locator.CurrentMutable.RegisterViewsForViewModels(typeof(IAssemblyScanningMarker).Assembly);

    ServiceProvider serviceProvider = services.BuildServiceProvider();

    serviceProvider.UseMicrosoftDependencyResolver();

    return serviceProvider;
  }

  private static void ConfigureServices(IServiceCollection services) {
    services.RegisterServicesFromType<IAssemblyScanningMarker>();
  }

  private void OnStartup(object sender, StartupEventArgs e) {
    var mainWindow = Locator.Current.GetService<MainWindow>();

    mainWindow?.Show();
  }
}
