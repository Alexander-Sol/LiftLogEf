using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using LiftLog.Data;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "liftlog.db");
builder.Services.AddDbContext<LiftLogDbContext>(o =>
    o.UseSqlite($"Data Source={dbPath}"));

// CORS (handy for browser-based testing and simulators)
builder.Services.AddCors(o =>
{
    o.AddPolicy("dev", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Register OpenAPI generation
builder.Services.AddOpenApi(options =>
{
    // optional: name your doc & tweak defaults
    options.AddDocumentTransformer((doc, ctx, ct) =>
    {
        doc.Info = new()
        {
            Title = "LiftLog API",
            Version = "v1",
            Description = "JSON REST API for the LiftLog app"
        };
        return Task.CompletedTask;
    });
});


var app = builder.Build();

app.UseCors("dev");

// Serve OpenAPI JSON at /openapi/v1.json (default name is "v1")
app.MapOpenApi();

// Nice interactive UI (Scalar) at /scalar/v1
app.MapScalarApiReference(options =>
{
    options.Title = "LiftLog API";
    options.EndpointPathPrefix = "/openapi"; // matches MapOpenApi()
    options.WithTheme(ScalarTheme.Kepler);   // optional
});

// optional: only expose UI in dev
//if (app.Environment.IsDevelopment()) app.MapScalarApiReference();
Console.WriteLine($"is app.Environment development?: {app.Environment.IsDevelopment()}");

app.Run();

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlite(connectionString));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();
// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseMigrationsEndPoint();
// }
// else
// {
//     app.UseExceptionHandler("/Home/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");
// app.MapRazorPages();

// app.Run();

