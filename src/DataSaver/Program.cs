var builder = WebApplication.CreateBuilder(args);

ConfigureCoreServices.ConfigureServices(builder.Configuration, builder.Services, builder.Logging);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<LinkContextSeed>>();

logger.LogInformation("Database migration running...");

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;

    try
    {
        var linkContext = scopedProvider.GetRequiredService<LinkContext>();
        if (linkContext.Database.IsSqlServer())
        {
            linkContext.Database.Migrate();
        }

        await LinkContextSeed.SeedAsync(linkContext, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred adding migrations to Database.");
    }
}

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
