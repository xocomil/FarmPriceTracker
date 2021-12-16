using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AssemblyScanning;
using FarmPriceTracker.ViewModels;
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
        this.BindCommand(ViewModel, vm => vm.FixButtonClicked, view => view.FixBtn)
          .DisposeWith(disposableRegistrations);

        this.OneWayBind(ViewModel, vm => vm.ErrorMessageQueue, view => view.ErrorSnackbar!.MessageQueue)
          .DisposeWith(disposableRegistrations);

        this.OneWayBind(ViewModel, vm => vm.FillTypesEmpty, view => view.DrawerHost!.IsTopDrawerOpen)
          .DisposeWith(disposableRegistrations);

        ViewModel.WhenAnyValue(vm => vm.FillTypes)
          .Select(fillTypes => fillTypes?.Count ?? 0)
          .BindTo(this, view => view.FillTypesBadge!.Badge)
          .DisposeWith(disposableRegistrations);

        ViewModel!.SettingsViewModel.WhenAnyValue(vm => vm.DataFolder)
          .Select(_ => Unit.Default)
          .InvokeCommand(ViewModel.LoadFillTypes)
          .DisposeWith(disposableRegistrations);
      }
    );
  }
}
