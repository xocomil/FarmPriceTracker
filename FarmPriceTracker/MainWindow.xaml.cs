using System;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using AssemblyScanning;
using FarmSimulator22Integrations.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace FarmPriceTracker;

/// <summary>
///   Interaction logic for MainWindow.xaml
/// </summary>
[Inject(serviceLifetime: ServiceLifetime.Singleton, provideFor: typeof(MainWindow))]
public partial class MainWindow : Window {
  public MainWindow() {
    InitializeComponent();

    var test = Fs22Map.CreateInstance();

    Console.WriteLine($"map: {JsonSerializer.Serialize(test.Map)}");
  }

  private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) {
    Application.Current.Shutdown();
  }
}
