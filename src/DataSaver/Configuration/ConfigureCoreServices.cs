namespace DataSaver.Configuration;
public static class ConfigureCoreServices
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services,
            ILoggingBuilder logging)
    {
        #region Logger

        logging.ClearProviders();
        logging.AddSerilog(
            new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger());

        #endregion

        #region DB

        services.AddDbContext<LinkContext>(context => context.UseSqlServer(configuration.GetConnectionString("LinkConnection")));

        #endregion

        #region Services
        services.AddControllersWithViews();

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddSession();

        #endregion
    }
}
