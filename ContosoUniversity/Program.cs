using ContosoUniversity.Data;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var useAzureSql = builder.Environment.IsProduction();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<SchoolContext>(options =>
{
    if (useAzureSql)
    {
        options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

foreach (var directory in new[] { "Content", "Scripts", "Uploads" })
{
    var path = Path.Combine(app.Environment.ContentRootPath, directory);
    Directory.CreateDirectory(path);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(path),
        RequestPath = $"/{directory}"
    });
}

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    DbInitializer.Initialize(context);
}

app.Run();

public partial class Program;
