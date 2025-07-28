namespace Neolution.Extensions.DataSeeding
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the data seeding functionality to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddDataSeeding(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddDataSeeding(services, assembly);
        }

        /// <summary>
        /// Adds the data seeding functionality to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="assembly">The assembly containing the seed implementations.</param>
        public static void AddDataSeeding(this IServiceCollection services, Assembly assembly)
        {
            services.AddTransient<ISeeder, Seeder>();
            Seeding.Instance.Configure(assembly);

            // Register ISeed implementations
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<ISeed>())
                .AsSelf()
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }
    }
}
