using DataSaver.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataSaver.Infrastructure.Data
{
    public sealed class LinkContextSeed
    {
        public static async Task SeedAsync(LinkContext linkContext, ILogger logger, int retry = 0)
        {
            var retryForAvailability = retry;

            try
            {
                if (!await linkContext.Links.AnyAsync())
                {
                    await linkContext.AddRangeAsync(
                        GetPreConfiguredLinks());

                    await linkContext.SaveChangesAsync();
                }

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
                new(3, 6, "American Psychological Association", "Psychology articles", "https://www.apa.org/")
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
                new("Speaking"),
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
