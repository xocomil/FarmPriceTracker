using AssemblyScanning;
using Microsoft.Extensions.DependencyInjection;

namespace FarmPriceTracker.Windows;

[Inject(provideFor: typeof(SettingsWindow), serviceLifetime: ServiceLifetime.Transient)]
public partial class SettingsWindow {
  public SettingsWindow() {
    InitializeComponent();
  }
}
