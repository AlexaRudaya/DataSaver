var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(_ =>
    _.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(_ =>
{
    _.Password.RequiredLength = 5;
    _.Password.RequireLowercase = true;
    _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
    _.Lockout.MaxFailedAccessAttempts = 5;
    _.SignIn.RequireConfirmedAccount = false;
});

const string CORS_POLICY = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS_POLICY,
                      _ =>
                      {
                          _.WithOrigins("https://localhost:7092")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(_ =>
{
    _.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "Identity_API", Version = "v1" });
});

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(_ =>
{
    _.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity_API");
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(CORS_POLICY);

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();