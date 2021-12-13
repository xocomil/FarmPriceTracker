using System.Reactive.Disposables;
using FarmPriceTracker.ViewModels;
using ReactiveUI;
using Splat;

namespace FarmPriceTracker.Controls;

public partial class Header : IEnableLogger {
  public Header() {
    InitializeComponent();
    ViewModel = Locator.Current.GetService<MainViewModel>();

    this.WhenActivated(
      disposableRegistrations => {
        this.BindCommand(ViewModel, vm => vm.ExitCommand, view => view.CloseBtn).DisposeWith(disposableRegistrations);
        this.BindCommand(ViewModel, vm => vm.OpenSettings, view => view.SettingsBtn)
          .DisposeWith(disposableRegistrations);
      }
    );
  }
}
