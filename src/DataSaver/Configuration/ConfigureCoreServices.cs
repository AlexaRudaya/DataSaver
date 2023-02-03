namespace DataSaver.Configuration;
public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        ////services.AddSingleton(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddAutoMapper(typeof(MapperProfile));

        return services;
    }
}
