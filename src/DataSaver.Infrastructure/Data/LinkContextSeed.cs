namespace DataSaver.Infrastructure.Data
{
    public sealed class LinkContextSeed
    {
        private readonly ILogger<LinkContextSeed> _logger;  

        public LinkContextSeed(ILogger<LinkContextSeed> logger)
        {
            _logger = logger;    
        }
        public static async Task SeedAsync(LinkContext linkContext, ILogger<LinkContextSeed> logger, int retry = 0)
        {
            var retryForAvailability = retry;

            try
            {
                if (!await linkContext.Topics.AnyAsync())
                {
                    await linkContext.AddRangeAsync(
                        GetPreConfiguredTopics());

                    await linkContext.SaveChangesAsync();
                }

                if (!await linkContext.Categories.AnyAsync())
                {
                    await linkContext.AddRangeAsync(
                        GetPreConfiguredCategories());

                    await linkContext.SaveChangesAsync();
                }

                if (!await linkContext.Links.AnyAsync())
                {
                    await linkContext.AddRangeAsync(
                        GetPreConfiguredLinks());

                    await linkContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;
                retryForAvailability++;

                logger.LogError(ex.Message);

                await SeedAsync(linkContext, logger, retryForAvailability);
            }
        }

        private static IEnumerable<Link> GetPreConfiguredLinks()
        {
            return new List<Link>
            {
                new(1, 1, "SQL Tutorial", "Giraffe Academy youTube course", "https://www.youtube.com/watch?v=xmwI6VB_wUM&list=PLLAZ4kZ9dFpMGXTKXsBM_ZNpJwowfsP49"),
                new(2, 4, "Learn English With TV Series", "Convenient way to gain vocabulary", "https://www.youtube.com/@LearnEnglishWithTVSeries"),
                new(3, 5, "American Psychological Association", "Psychology articles", "https://www.apa.org/"),
                new(1, 1, "SQL Murder Mystery", "A game for learning SQL", "https://mystery.knightlab.com/"),
                new(2, 4, "Cambridge Dictionary", "The best dictionary with definitions and audio pronunciations of words, phrases, and idioms", "https://dictionary.cambridge.org"),
                new(1, 3, "Intro to ASP.NET Core Razor Pages", "IAmTimCorey youTube tutorial", "https://www.youtube.com/watch?v=68towqYcQlY&list=PLsXTqdzHSirmPIbxqDoZRn867l6aVYPmd&index=20&t=1886s"),
                new(1, 2, "Repository Pattern With EF Core", "Milan Jovanović youTube tutorial", "https://www.youtube.com/watch?v=h4KIngWVpfU&list=PLsXTqdzHSirmPIbxqDoZRn867l6aVYPmd&index=7&t=11s")
            };
        }

        private static IEnumerable<Topic> GetPreConfiguredTopics()
        {
            return new List<Topic>
            { 
                new("SQL"),
                new("EF"),
                new("RazorPages"),
                new("Vocabulary"),
                new("Facts")
            };
        }

        private static IEnumerable<Category> GetPreConfiguredCategories()
        {
            return new List<Category>
            {
                new("IT"),
                new("English"),
                new("Psychology")
            };
        }
    }
}
