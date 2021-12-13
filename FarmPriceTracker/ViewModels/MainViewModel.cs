using System.Reactive;
using System.Windows;
using AssemblyScanning;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainViewModel))]
public class MainViewModel : ReactiveObject {
  public ReactiveCommand<Unit, Unit> ExitCommand { get; } = ReactiveCommand.Create(
    () => { Application.Current?.Shutdown(); }
  );

  public ReactiveCommand<Unit, Unit> OpenSettings { get; } = ReactiveCommand.Create(OpenSettingsWindow);

  public SettingsViewModel SettingsViewModel { get; }

  public MainViewModel(SettingsViewModel settingsViewModel) {
    SettingsViewModel = settingsViewModel;
  }


  private static void OpenSettingsWindow() {
    SettingsViewModel.OpenSettings();
  }
 }
