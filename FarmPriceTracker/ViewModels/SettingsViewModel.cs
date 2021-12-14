using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using AssemblyScanning;
using FarmPriceTracker.Windows;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Splat;

namespace FarmPriceTracker.ViewModels;

[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(SettingsViewModel))]
public class SettingsViewModel : ReactiveValidationObject {
  private readonly ObservableAsPropertyHelper<bool> _dataFolderValid;
  private string _dataFolder = @"c:\test\test\test";

  public SettingsViewModel() {
    this.ValidationRule(
      vm => vm.DataFolder,
      folder => !string.IsNullOrWhiteSpace(folder),
      "You must specify the location of the data folder."
    );

    this.ValidationRule(vm => vm.DataFolder, Directory.Exists, "The data folder does not exist.");

    _dataFolderValid = this.IsValid()
      // .ObserveOn(RxApp.MainThreadScheduler)
      .Do(model => Trace.WriteLine($"valid: {model}"))
      // .Where(valid => !valid)
      .Select(valid => !valid && (GetErrors(nameof(DataFolder)) as IEnumerable<string>).Any())
      .Do(model => Trace.WriteLine($"after select: {model}"))
      .ToProperty(this, x => x.DataFolderValid, scheduler: RxApp.MainThreadScheduler);

    BrowseForDataFolder = ReactiveCommand.Create<Window>(OpenFileDialogForDataFolder);

    this.WhenAnyValue(x => x.DataFolderValid).Subscribe(valid => Trace.WriteLine($"whenanyvalue valid: {valid}"));
  }

  public bool DataFolderValid => _dataFolderValid.Value;

  public string DataFolder {
    get => _dataFolder;
    set => this.RaiseAndSetIfChanged(ref _dataFolder, value);
  }

  public ReactiveCommand<Window, Unit> CloseSettings { get; } = ReactiveCommand.Create<Window>(CloseSettingsWindow);
  public ReactiveCommand<Window, Unit> BrowseForDataFolder { get; }

  public ValidationContext ValidationContext { get; } = new();

  private void OpenFileDialogForDataFolder(Window parent) {
    var dialog = new FolderBrowserDialog();

    dialog.InitialDirectory = DataFolder;
    dialog.Description = "Location of Data Folder";
    dialog.UseDescriptionForTitle = true;

    if ( dialog.ShowDialog() == DialogResult.OK ) {
      DataFolder = dialog.SelectedPath;
    }
  }

  private static void CloseSettingsWindow(Window window) {
    window.Close();
  }

  public static void OpenSettings() {
    var settingsWindow = Locator.Current.GetService<SettingsWindow>();

    settingsWindow?.ShowDialog();
  }
}
