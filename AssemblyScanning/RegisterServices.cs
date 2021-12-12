using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AssemblyScanning;

public static class RegisterServices {
  public static void RegisterServicesFromType<T>(this IServiceCollection services) {
    RegisterServicesFromAssemblies(services, typeof(T).Assembly);
  }

  public static void RegisterServicesFromTypes(this IServiceCollection service, params Type[] types) {
    if ( types == null ) {
      throw new ArgumentNullException(nameof(types));
    }

    var assemblies = types.Select(t => t.Assembly).ToArray();

    RegisterServicesFromAssemblies(service, assemblies);
  }

  private static void RegisterServicesFromAssemblies(this IServiceCollection services, params Assembly[] assemblies) {
    if ( services == null ) {
      throw new ArgumentNullException(nameof(services));
    }

    if ( assemblies == null ) {
      throw new ArgumentNullException(nameof(assemblies));
    }

    var injectedTypes = assemblies.SelectMany(assembly =>
      assembly.DefinedTypes.Where(t => t.GetCustomAttributes(typeof(InjectAttribute), false).FirstOrDefault() != null));

    foreach(var injectedType in injectedTypes) {
      var attributeData = injectedType.GetCustomAttributes(typeof(InjectAttribute), false).First() as InjectAttribute;

      if ( attributeData == null ) {
        throw new InvalidOperationException($"Could not find InjectAttribute for {injectedType}");
      }

      if ( attributeData.ProvideFor != null ) {
        services.Add(new ServiceDescriptor(attributeData.ProvideFor, injectedType, attributeData.ServiceLifetime));

        return;
      }

      foreach ( var implementedInterface in injectedType.ImplementedInterfaces ) {
        services.Add(new ServiceDescriptor(implementedInterface, injectedType, attributeData.ServiceLifetime));
      }
    }
  }
}
