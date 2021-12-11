using Microsoft.Extensions.DependencyInjection;

namespace AssemblyScanning;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute: Attribute {
  public ServiceLifetime ServiceLifetime { get; init; }
  public Type ProvideForInterface { get; init; }

  public InjectAttribute(Type provideForInterface = null, ServiceLifetime serviceLifetime = ServiceLifetime.Transient) {
    ServiceLifetime = serviceLifetime;

    if ( provideForInterface is { IsInterface: false } ) {
      throw new ArgumentException("ProvideForInterface must be an interface.", nameof(provideForInterface));
    }

    ProvideForInterface = provideForInterface;
  }
}
