using System;
using System.Reactive.Disposables;
using FarmPriceTracker.ViewModels;
using Splat;

namespace FarmPriceTracker.Windows;

public class Settings {
  public Settings() {
    InitializeComponent();
    ViewModel = Locator.Current.GetService<SettingsViewModel>();

    this.WhenActivated(
      disposableRegistrations => {
        this.BindCommand(ViewModel, vm => vm.CloseSettings, view => view.CloseBtn).DisposeWith(disposableRegistrations);
        this.BindCommand(ViewModel, vm => vm.BrowseForDataFolder, view => view.BrowseBtn)
          .DisposeWith(disposableRegistrations);
        this.BindCommand(ViewModel, vm => vm.GetFs22Location, view => view.Fs22LocationBtn)
          .DisposeWith(disposableRegistrations);

        this.Bind(ViewModel, vm => vm.DataFolder, view => view.DataFolder!.Text).DisposeWith(disposableRegistrations);

        DisposableMixins.DisposeWith<IDisposable>(
          this.WhenAnyValue(x => x.ViewModel!.DataFolderValid)
            .Select(valid => !valid)
            .BindTo(this, x => x.ValidFolder!.Visibility),
          disposableRegistrations
        );

        this.OneWayBind(ViewModel, vm => vm.DataFolderValid, view => view.FolderCog!.Visibility)
          .DisposeWith(disposableRegistrations);
      }
    );
  }
}
