using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Forms;
using AssemblyScanning;
using FarmPriceTracker.Windows;
using FarmPriceTracker.WinFormsCompat;
using GameLibrary;
using GameLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Splat;

namespace FarmPriceTracker.ViewModels;

[Inject(
  serviceLifetime: ServiceLifetime.Singleton,
  provideFor: typeof(SettingsViewModel),
  factoryFunctionName: nameof(CreateInstance)
)]
[Serializable]
public class SettingsViewModel : ReactiveValidationObject {
  private static readonly string SettingsFilePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    "/Games/.FarmPriceTracker/settings.json"
  );

  private readonly ObservableAsPropertyHelper<bool> _dataFolderValid;
  private string _dataFolder = @"c:\test\test\test";

  [JsonConstructor]
  public SettingsViewModel() {
    this.ValidationRule(
      vm => vm.DataFolder,
      folder => !string.IsNullOrWhiteSpace(folder),
      "You must specify the location of the data folder."
    );

    this.ValidationRule(vm => vm.DataFolder, Directory.Exists, "The data folder does not exist.");

    _dataFolderValid = this.IsValid()
      .Select(valid => !valid && (GetErrors(nameof(DataFolder)) as IEnumerable<string> ?? Array.Empty<string>()).Any())
      .ToProperty(this, x => x.DataFolderValid, scheduler: RxApp.MainThreadScheduler);

    BrowseForDataFolder = ReactiveCommand.Create<Window>(OpenFileDialogForDataFolder);
    CloseSettings = ReactiveCommand.Create<Window>(CloseSettingsWindow);
    GetFs22Location = ReactiveCommand.Create(FindFs22Location);
  }

  public bool DataFolderValid => _dataFolderValid.Value;

  public string DataFolder {
    get => _dataFolder;
    set => this.RaiseAndSetIfChanged(ref _dataFolder, value);
  }

  public ReactiveCommand<Window, Unit> CloseSettings { get; }
  public ReactiveCommand<Window, Unit> BrowseForDataFolder { get; }
  public ReactiveCommand<Unit, Unit> GetFs22Location { get; }

  public new ValidationContext ValidationContext { get; } = new();

  private void FindFs22Location() {
    var steamHelper = Locator.Current.GetService<SteamHelper>();

    SteamFindResult? result = steamHelper?.SteamInstallLocation(SteamHelper.FarmingSimulator22SteamId);

    if ( result is { Found: true, GameLocation: { } } ) {
      DataFolder = Path.Combine(result.GameLocation, "data");

      return;
    }

    var mainWindowViewModel = Locator.Current.GetService<MainViewModel>();

    using IDisposable? tmp = mainWindowViewModel?.ShowErrorMessage.Execute(
        result?.ErrorMessage ?? "Something went wrong while trying to get the FS22 install location."
      )
      .Subscribe();
  }

  private void OpenFileDialogForDataFolder(Window parent) {
    var dialog = new FolderBrowserDialog {
      InitialDirectory = DataFolder, Description = "Location of Data Folder", UseDescriptionForTitle = true
    };

    if ( dialog.ShowDialog(parent.GetIWin32Window()) == DialogResult.OK ) {
      DataFolder = dialog.SelectedPath ?? string.Empty;
    }
  }

  private void CloseSettingsWindow(Window window) {
    window.Close();
    SaveSettingsFile();
  }

  public static void OpenSettings() {
    var settingsWindow = Locator.Current.GetService<SettingsWindow>();

    settingsWindow?.ShowDialog();
  }

  private void SaveSettingsFile() {
    string settingsDirectory = Path.GetDirectoryName(SettingsFilePath)!;

    if ( !Directory.Exists(settingsDirectory) ) {
      Directory.CreateDirectory(settingsDirectory);
    }

    File.WriteAllTextAsync(SettingsFilePath, JsonSerializer.Serialize(this));
  }

  private static SettingsViewModel? LoadSettingsFile() {
    if ( !File.Exists(SettingsFilePath) ) {
      return null;
    }

    string json = File.ReadAllText(SettingsFilePath);
    return JsonSerializer.Deserialize<SettingsViewModel>(json);
  }

  public static SettingsViewModel CreateInstance() {
    SettingsViewModel? settingsFromFile = LoadSettingsFile();

    return settingsFromFile ?? new SettingsViewModel();
  }
}
