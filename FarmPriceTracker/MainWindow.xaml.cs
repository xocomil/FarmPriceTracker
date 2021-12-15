using System;
using System.Reactive.Disposables;
using System.Text.Json;
using AssemblyScanning;
using FarmPriceTracker.ViewModels;
using FarmSimulator22Integrations.Parsers;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace FarmPriceTracker;

/// <summary>
///   Interaction logic for MainWindow.xaml
/// </summary>
[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainWindow))]
public partial class MainWindow {
  public MainWindow() {
    InitializeComponent();
    ViewModel = Locator.Current.GetService<MainViewModel>();

    this.WhenActivated(
      disposableRegistrations => {
        this.OneWayBind(ViewModel, vm => vm.ErrorMessageQueue, view => view.ErrorSnackbar!.MessageQueue)
          .DisposeWith(disposableRegistrations);
      }
    );

    var test = Fs22Map.CreateInstance();

    Console.WriteLine($"map: {JsonSerializer.Serialize(test.Map)}");
  }
}
