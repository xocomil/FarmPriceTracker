using Microsoft.Extensions.DependencyInjection;

namespace AssemblyScanning;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute {
  public InjectAttribute(
    Type? provideFor = null,
    ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
    string? factoryFunctionName = null
  ) {
    ServiceLifetime = serviceLifetime;

    if ( provideFor is { IsInterface: false, IsTypeDefinition: false } ) {
      throw new ArgumentException($"{provideFor} must be an interface or a type.", nameof(provideFor));
    }

    ProvideFor = provideFor;

    FactoryFunctionName = factoryFunctionName;
  }

  public string? FactoryFunctionName { get; }

  public ServiceLifetime ServiceLifetime { get; }
  public Type? ProvideFor { get; }
}
