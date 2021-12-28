var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(
    (context, next) =>
    {
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        return next();
    });

app.UseStatusCodePagesWithReExecute("/Shared/Error");
app.UseWebOptimizer();

app.UseStaticFilesWithCache();

if (builder.Configuration.GetValue<bool>("forcessl"))
{
    app.UseHttpsRedirection();
}

app.UseMetaWeblog("/metaweblog");
app.UseAuthentication();

app.UseOutputCaching();
app.UseWebMarkupMin();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(
    endpoints =>
    {
        endpoints.MapControllerRoute("default", "{controller=Blog}/{action=Index}/{id?}");
    });


app.Run();
