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

    Assembly[] assemblies = types.Select(t => t.Assembly).ToArray();

    RegisterServicesFromAssemblies(service, assemblies);
  }

  private static void RegisterServicesFromAssemblies(this IServiceCollection services, params Assembly[] assemblies) {
    if ( services == null ) {
      throw new ArgumentNullException(nameof(services));
    }

    if ( assemblies == null ) {
      throw new ArgumentNullException(nameof(assemblies));
    }

    IEnumerable<TypeInfo> injectedTypes = assemblies.SelectMany(
      assembly => assembly.DefinedTypes.Where(
        t => t.GetCustomAttributes(typeof(InjectAttribute), false).FirstOrDefault() != null
      )
    );

    foreach ( TypeInfo injectedType in injectedTypes ) {
      if ( injectedType.GetCustomAttributes(typeof(InjectAttribute), false)
            .First() is not InjectAttribute attributeData ) {
        throw new InvalidOperationException($"Could not find InjectAttribute for {injectedType}");
      }

      if ( attributeData.ProvideFor != null ) {
        services.Add(CreateDescriptorFromAttribute(attributeData, injectedType));

        continue;
      }

      foreach ( Type implementedInterface in injectedType.ImplementedInterfaces ) {
        services.Add(CreateDescriptor(implementedInterface, injectedType, attributeData));
      }
    }
  }

  private static ServiceDescriptor CreateDescriptorFromAttribute(InjectAttribute attribute, Type injectedType) =>
    CreateDescriptor(attribute.ProvideFor!, injectedType, attribute);

  private static ServiceDescriptor CreateDescriptor(
    Type implementedInterface,
    Type injectedType,
    InjectAttribute attribute
  ) {
    Func<IServiceProvider, object>? factoryFunction = GetFactoryFunction(injectedType, attribute.FactoryFunctionName);

    return factoryFunction != null
      ? new ServiceDescriptor(implementedInterface, factoryFunction, attribute.ServiceLifetime)
      : new ServiceDescriptor(implementedInterface, injectedType, attribute.ServiceLifetime);
  }

  private static Func<IServiceProvider, object>? GetFactoryFunction(Type injectedType, string? factoryName) {
    if ( string.IsNullOrWhiteSpace(factoryName) ) {
      return null;
    }

    MethodInfo? methodInfo = injectedType.GetMethod(factoryName);

    return methodInfo?.ReturnType is not null ? _ => methodInfo.Invoke(null, null)! : null;
  }
}
