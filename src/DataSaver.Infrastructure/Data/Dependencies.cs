namespace DataSaver.Infrastructure.Data
{
    public sealed class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<LinkContext>(context => context.UseSqlServer(configuration.GetConnectionString("LinkConnection")));
        }
    }
}
