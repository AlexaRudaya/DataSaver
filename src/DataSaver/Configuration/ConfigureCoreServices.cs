using Microsoft.AspNetCore.Authentication.Cookies;

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

        #region Identity

        services.AddDbContext<ApplicationDbContext>(_ =>
            _.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
                options.LogoutPath = "/Accounts/LogOff";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(_ =>
        {
            _.Password.RequiredLength = 5;
            _.Password.RequireLowercase = true;
            _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
            _.Lockout.MaxFailedAccessAttempts = 5;
            _.SignIn.RequireConfirmedAccount = false;
        });

        #endregion

        #region LinkPreview_API

        services.AddHttpClient<ISetLinkPreviewService, SetLinkPreviewService>()
                .AddTransientHttpErrorPolicy(_ => _.WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(2)))
                .AddTransientHttpErrorPolicy(_ => _.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)));

        services.Configure<LinkPreviewOptions>(configuration.GetSection("LinkPreview"));

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
        services.AddScoped<ISetLinkPreviewService, SetLinkPreviewService>();
        services.AddAutoMapper(typeof(MapperProfile));

        services.AddSession();

        #endregion
    }
}
