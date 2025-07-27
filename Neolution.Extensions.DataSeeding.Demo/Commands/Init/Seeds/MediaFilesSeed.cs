namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Uploads sample media files like images and documents
    /// Has no dependencies and can run independently
    /// </summary>
    public class MediaFilesSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<MediaFilesSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFilesSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MediaFilesSeed(ILogger<MediaFilesSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Uploading sample media files: logo.png, hero-image.jpg, sample-document.pdf...");
            await Task.Delay(300);
            this.logger.LogInformation("Sample media files uploaded to media library");
        }
    }
}
