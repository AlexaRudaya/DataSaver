using DataSaver.ApplicationCore.Interfaces.ILogger;
using DataSaver.ApplicationCore.Interfaces.IRepository;
using DataSaver.ApplicationCore.Interfaces.IService;
using DataSaver.Infrastructure.Logger;
using DataSaver.Infrastructure.Mapper;
using DataSaver.Infrastructure.Repositories;
using DataSaver.Infrastructure.Services;
using DataSaver.Middlewares;

namespace DataSaver.Configuration
{
    public static class ConfigureCoreServices
    {
        internal static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
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
}
