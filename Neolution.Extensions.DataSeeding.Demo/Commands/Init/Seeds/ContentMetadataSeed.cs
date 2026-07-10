namespace Neolution.Extensions.DataSeeding.Demo.Commands.Init.Seeds
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Neolution.Extensions.DataSeeding.Abstractions;

    /// <summary>
    /// Creates content metadata and SEO tags
    /// Has no dependencies but is used by ContentSeed for metadata assignment
    /// </summary>
    public class ContentMetadataSeed : ISeed
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ContentMetadataSeed> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentMetadataSeed"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ContentMetadataSeed(ILogger<ContentMetadataSeed> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            this.logger.LogInformation("Creating SEO metadata templates: title patterns, meta descriptions...");
            await Task.Delay(150);
            this.logger.LogInformation("Setting up content tags: technology, business, tutorial, news...");
            await Task.Delay(100);
            this.logger.LogInformation("Configuring schema.org structured data templates...");
            await Task.Delay(100);
            this.logger.LogInformation("Content metadata and SEO configuration completed");
        }
    }
}
