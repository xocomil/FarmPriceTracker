using System.Reflection;

namespace FarmPriceTracker;

/// <summary>
///   Used to mark for assembly scanning purposes only.
/// </summary>
public interface IAssemblyScanningMarker {
  void ScanAssembly(Assembly assembly);
}
