using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer.AutoMapper
{
    public static class AutoMapperExtension
    {
        /// <summary>
        /// Use the static registration method of Mapper.Initialize. Defaults to true.
        /// When false, an instance of a MapperConfiguration object is registered instead.
        /// </summary>
        public static bool UseStaticRegistration { get; set; } = true;

        public static IServiceCollection AddAutoMapper(this IServiceCollection services,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector = null, ServiceLifetime mapperLifetime = ServiceLifetime.Scoped)
        {
            return services.AddAutoMapper(null, AppDomain.CurrentDomain.GetAssemblies(), serviceLifetimeSelector, mapperLifetime);
        }

        private static readonly Action<IMapperConfigurationExpression> DefaultConfig = cfg => { };

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
            => AddAutoMapperClasses(services, null, assemblies);

        public static IServiceCollection AddAutoMapper(
            this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, params Assembly[] assemblies)
            => AddAutoMapperClasses(services, additionalInitAction, assemblies);

        public static IServiceCollection AddAutoMapper(
            this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector, ServiceLifetime mapperLifetime,
            params Assembly[] assemblies)
            => AddAutoMapperClasses(services, additionalInitAction, assemblies, serviceLifetimeSelector, mapperLifetime);

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Assembly> assemblies,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector = null, ServiceLifetime mapperLifetime = ServiceLifetime.Scoped)
            => AddAutoMapperClasses(services, additionalInitAction, assemblies, serviceLifetimeSelector, mapperLifetime);

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, IEnumerable<Assembly> assemblies,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector = null, ServiceLifetime mapperLifetime = ServiceLifetime.Scoped)
            => AddAutoMapperClasses(services, null, assemblies, serviceLifetimeSelector, mapperLifetime);

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(services, null, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly));
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector, ServiceLifetime mapperLifetime,
            params Type[] profileAssemblyMarkerTypes)
        {
            return AddAutoMapperClasses(services, additionalInitAction, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly), serviceLifetimeSelector, mapperLifetime);
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Type> profileAssemblyMarkerTypes,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector = null, ServiceLifetime mapperLifetime = ServiceLifetime.Scoped)
        {
            return AddAutoMapperClasses(services, additionalInitAction, profileAssemblyMarkerTypes.Select(t => t.GetTypeInfo().Assembly), serviceLifetimeSelector, mapperLifetime);
        }

        private static ServiceDescriptor CreateDescriptor(Type serviceType, ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(serviceType, serviceType, lifetime);
        }

        private static IServiceCollection AddAutoMapperClasses(IServiceCollection services, Action<IMapperConfigurationExpression> additionalInitAction, IEnumerable<Assembly> assembliesToScan,
            Func<TypeInfo, ServiceLifetime> serviceLifetimeSelector = null, ServiceLifetime mapperLifetime = ServiceLifetime.Scoped)
        {
            additionalInitAction = additionalInitAction ?? DefaultConfig;
            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var allTypes = assembliesToScan
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            var profiles = allTypes
                .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();


            void ConfigAction(IMapperConfigurationExpression cfg)
            {
                additionalInitAction(cfg);

                foreach (var profile in profiles.Select(t => t.AsType()))
                {
                    cfg.AddProfile(profile);
                }
            }

            IConfigurationProvider config;
            if (UseStaticRegistration)
            {
                Mapper.Initialize(ConfigAction);
                config = Mapper.Configuration;
            }
            else
            {
                config = new MapperConfiguration(ConfigAction);
            }

            serviceLifetimeSelector = serviceLifetimeSelector ?? (typeInfo => ServiceLifetime.Transient);


            var openTypes = new[]
            {
                typeof(IValueResolver<,,>),
                typeof(IMemberValueResolver<,,,>),
                typeof(ITypeConverter<,>),
                typeof(IMappingAction<,>)
            };
            foreach (var type in openTypes.SelectMany(openType => allTypes
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.AsType().ImplementsGenericInterface(openType))))
            {
                services.Add(CreateDescriptor(type.AsType(), serviceLifetimeSelector(type)));
            }

            services.AddSingleton(config);
            services.Add(new ServiceDescriptor(
                typeof(IMapper), sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService),
                mapperLifetime
            ));
            return services;
        }

        private static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return type.IsGenericType(interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => @interface.IsGenericType(interfaceType));
        }

        private static bool IsGenericType(this Type type, Type genericType)
            => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}
