using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using AssemblyScanning;
using FarmSimulator22Integrations.Models;
using FarmSimulator22Integrations.Parsers;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainViewModel))]
public class MainViewModel : ReactiveObject {
  private readonly ObservableAsPropertyHelper<bool> _fillTypesEmpty;
  public readonly SnackbarMessageQueue ErrorMessageQueue = new();

  private List<Fs22FillTypePriceData> _fillTypes;

  public MainViewModel(SettingsViewModel settingsViewModel) {
    SettingsViewModel = settingsViewModel;

    FixButtonClicked = ReactiveCommand.Create<TabControl>(FixButtonCommandHandler);

    ShowErrorMessage = ReactiveCommand.Create<string>(EnqueueError);
    LoadFillTypes = ReactiveCommand.Create(LoadFillTypesFromData);

    _fillTypesEmpty = this.WhenAnyValue(vm => vm.FillTypes)
      .Select(fillTypes => fillTypes is null || !fillTypes.Any())
      .ToProperty(this, x => x.FillTypesEmpty, scheduler: RxApp.MainThreadScheduler);
  }

  public bool FillTypesEmpty => _fillTypesEmpty.Value;

  public List<Fs22FillTypePriceData> FillTypes {
    get => _fillTypes;
    set => this.RaiseAndSetIfChanged(ref _fillTypes, value);
  }

  public ReactiveCommand<Unit, Unit> ExitCommand { get; } = ReactiveCommand.Create(
    () => { Application.Current?.Shutdown(); }
  );

  public ReactiveCommand<string, Unit> ShowErrorMessage { get; }

  public ReactiveCommand<Unit, Unit> LoadFillTypes { get; }

  public ReactiveCommand<TabControl, Unit> FixButtonClicked { get; }

  public SettingsViewModel SettingsViewModel { get; }

  private void FixButtonCommandHandler(TabControl tabControl) {
    tabControl.Dispatcher?.BeginInvoke(() => tabControl.SelectedIndex = 1);

    using IDisposable sub = SettingsViewModel.FocusDataFolder.Execute().Subscribe();
  }

  private void LoadFillTypesFromData() {
    var map = Fs22Map.CreateInstance(SettingsViewModel.DataFolder);

    FillTypes = map.FillTypes;
  }

  private void EnqueueError(string message) {
    ErrorMessageQueue.Enqueue(message);
  }
}
