using System.Windows;
using AssemblyScanning;
using GameLibrary;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace FarmPriceTracker;

/// <summary>
///   Interaction logic for App.xaml
/// </summary>
public partial class App {
  public App() {
    InitializeSplat();
  }

  private static void InitializeSplat() {
    var services = new ServiceCollection();

    services.UseMicrosoftDependencyResolver();
    IMutableDependencyResolver resolver = Locator.CurrentMutable;

    resolver.InitializeSplat();
    resolver.InitializeReactiveUI();

    ConfigureServices(services);

    Locator.CurrentMutable.RegisterViewsForViewModels(typeof(IAssemblyScanningMarker).Assembly);

    ServiceProvider serviceProvider = services.BuildServiceProvider();

    serviceProvider.UseMicrosoftDependencyResolver();
  }

  private static void ConfigureServices(IServiceCollection services) {
    DependencyInjection.AddDependencies(services);
    services.RegisterServicesFromType<IAssemblyScanningMarker>();
  }

  private void OnStartup(object sender, StartupEventArgs e) {
    var mainWindow = Locator.Current.GetService<MainWindow>();

    mainWindow?.Show();
  }
}
