using AssemblyScanning;
using GameFinder.StoreHandlers.Steam;
using Microsoft.Extensions.DependencyInjection;

namespace GameLibrary;

public static class DependencyInjection {
  public static void AddDependencies(IServiceCollection services) {
    services.AddTransient<SteamHandler>();
    services.RegisterServicesFromType<IAssemblyScanningMarker>();
  }
}
