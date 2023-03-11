var builder = WebApplication.CreateBuilder(args);

ConfigureCoreServices.ConfigureServices(builder.Configuration, builder.Services, builder.Logging);

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<LinkContextSeed>>();

//app.Logger.LogInformation("Database migration running...");
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

        //await LinkContextSeed.SeedAsync(linkContext, app.Logger);
        await LinkContextSeed.SeedAsync(linkContext, logger);
    }
    catch (Exception ex)
    {
        //app.Logger.LogError(ex, "An error occurred adding migrations to Database.");
        logger.LogError(ex, "An error occurred adding migrations to Database.");

    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
