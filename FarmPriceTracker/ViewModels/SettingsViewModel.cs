using System.Reactive;
using System.Windows;
using AssemblyScanning;
using FarmPriceTracker.Windows;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(SettingsViewModel))]
public class SettingsViewModel {
  public ReactiveCommand<Window, Unit> CloseSettings { get; } = ReactiveCommand.Create<Window>(CloseSettingsWindow);

  private static void CloseSettingsWindow(Window window) {
    window.Close();
  }

  public static void OpenSettings() {
    var settingsWindow = Locator.Current.GetService<SettingsWindow>();

    settingsWindow?.Show();
  }
}
