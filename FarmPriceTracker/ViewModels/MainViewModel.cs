using System.Reactive;
using System.Windows;
using AssemblyScanning;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainViewModel))]
public class MainViewModel : ReactiveObject {
  public readonly SnackbarMessageQueue ErrorMessageQueue = new();

  public MainViewModel(SettingsViewModel settingsViewModel) {
    SettingsViewModel = settingsViewModel;

    ShowErrorMessage = ReactiveCommand.Create<string>(EnqueueError);
  }

  public ReactiveCommand<Unit, Unit> ExitCommand { get; } = ReactiveCommand.Create(
    () => { Application.Current?.Shutdown(); }
  );

  public ReactiveCommand<Unit, Unit> OpenSettings { get; } = ReactiveCommand.Create(OpenSettingsWindow);

  public ReactiveCommand<string, Unit> ShowErrorMessage { get; }

  public SettingsViewModel SettingsViewModel { get; }

  private void EnqueueError(string message) {
    ErrorMessageQueue.Enqueue(message);
  }

  private static void OpenSettingsWindow() {
    SettingsViewModel.OpenSettings();
  }
}
