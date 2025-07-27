namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Sets up media processing configuration and creates thumbnails for uploaded media files
    /// Depends on MediaFilesSeed to have files available for processing
    /// </summary>
    public class MediaProcessingSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MediaProcessingSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaProcessingSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MediaProcessingSeed(ILogger<MediaProcessingSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public Type[] DependsOnTypes => new[]
        {
            typeof(MediaFilesSeed),
        };

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Configuring media processing settings: thumbnail sizes, compression levels...");
            await Task.Delay(200);
            this.logger.LogInformation("Generating thumbnails for uploaded media files...");
            await Task.Delay(300);
            this.logger.LogInformation("Media processing configuration completed and thumbnails generated");
        }
    }
}
