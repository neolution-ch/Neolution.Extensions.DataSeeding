namespace Neolution.Extensions.DataSeeding.Demo
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The startup class, composition root for the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarLint", "S2325", Justification = "Method is part of ICompositionRoot interface contract")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataSeeding();
        }
    }
}
