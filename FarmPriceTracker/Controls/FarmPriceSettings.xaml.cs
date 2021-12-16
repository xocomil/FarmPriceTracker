using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FarmPriceTracker.ViewModels;
using ReactiveUI;
using Splat;

namespace FarmPriceTracker.Controls;

public partial class FarmPriceSettings {
  public FarmPriceSettings() {
    InitializeComponent();
    ViewModel = Locator.Current.GetService<SettingsViewModel>();

    this.WhenActivated(
      disposableRegistrations => {
        this.BindCommand(ViewModel, vm => vm.SaveSettings, view => view.SaveBtn).DisposeWith(disposableRegistrations);
        this.BindCommand(ViewModel, vm => vm.BrowseForDataFolder, view => view.BrowseBtn)
          .DisposeWith(disposableRegistrations);
        this.BindCommand(ViewModel, vm => vm.GetFs22Location, view => view.Fs22LocationBtn)
          .DisposeWith(disposableRegistrations);

        this.Bind(ViewModel, vm => vm.DataFolder, view => view.DataFolder!.Text).DisposeWith(disposableRegistrations);

        this.WhenAnyValue(x => x.ViewModel!.DataFolderValid)
          .Select(valid => !valid)
          .BindTo(this, x => x.ValidFolder!.Visibility)
          .DisposeWith(disposableRegistrations);

        this.OneWayBind(ViewModel, vm => vm.DataFolderValid, view => view.FolderCog!.Visibility)
          .DisposeWith(disposableRegistrations);

        this.WhenAnyObservable(x => x.ViewModel.FocusDataFolder)
          .Subscribe(
            _ => {
              DataFolder.Focus();
              DataFolder.SelectAll();
            }
          )
          .DisposeWith(disposableRegistrations);
      }
    );
  }
}
