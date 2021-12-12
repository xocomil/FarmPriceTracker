using System.Windows;
using AssemblyScanning;
using Microsoft.Extensions.DependencyInjection;

namespace FarmPriceTracker;

/// <summary>
///   Interaction logic for App.xaml
/// </summary>
public partial class App {
  private readonly ServiceProvider _serviceProvider;

  public App() {
    var services = new ServiceCollection();
    ConfigureServices(services);
    _serviceProvider = services.BuildServiceProvider();
  }

  private static void ConfigureServices(IServiceCollection services) {
    services.RegisterServicesFromType<IAssemblyScanningMarker>();
  }

  private void OnStartup(object sender, StartupEventArgs e) {
    var mainWindow = _serviceProvider.GetService<MainWindow>();

    mainWindow?.Show();
  }
}
