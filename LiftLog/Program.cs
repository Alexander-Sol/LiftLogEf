using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LiftLog.Data;


var builder = WebApplication.CreateBuilder(args);
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "liftlog.db");
builder.Services.AddDbContext<LiftLogDbContext>(o =>
    o.UseSqlite($"Data Source={dbPath}"));
var app = builder.Build();
app.MapGet("/", () => "OK");
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

