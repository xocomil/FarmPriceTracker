using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using AssemblyScanning;
using DataTypes.Interfaces;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainViewModel))]
public class MainViewModel : ReactiveObject {
  private readonly ObservableAsPropertyHelper<bool> _fillTypesEmpty;
  public readonly SnackbarMessageQueue ErrorMessageQueue = new();

  private Collection<IFillTypePriceData>? _fillTypes;

  public MainViewModel(SettingsViewModel settingsViewModel) {
    SettingsViewModel = settingsViewModel;

    ShowErrorMessage = ReactiveCommand.Create<string>(EnqueueError);

    _fillTypesEmpty = this.WhenAnyValue(vm => vm.FillTypes)
      .Select(fillTypes => fillTypes is null || !fillTypes.Any())
      .ToProperty(this, x => x.FillTypesEmpty, scheduler: RxApp.MainThreadScheduler);
  }

  public bool FillTypesEmpty => _fillTypesEmpty.Value;

  public Collection<IFillTypePriceData>? FillTypes {
    get => _fillTypes;
    set => this.RaiseAndSetIfChanged(ref _fillTypes, value);
  }

  public ReactiveCommand<Unit, Unit> ExitCommand { get; } = ReactiveCommand.Create(
    () => { Application.Current?.Shutdown(); }
  );

  public ReactiveCommand<string, Unit> ShowErrorMessage { get; }

  public SettingsViewModel SettingsViewModel { get; }

  private void EnqueueError(string message) {
    ErrorMessageQueue.Enqueue(message);
  }
}
