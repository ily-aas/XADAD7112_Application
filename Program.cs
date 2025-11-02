using APDS_POE.Repositories;
using APDS_POE.Services;
using Microsoft.EntityFrameworkCore;
using XADAD7112_Application.Repositories;
using XADAD7112_Application.Services;

var builder = WebApplication.CreateBuilder(args);

//DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Auth
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<JwtAuthentication>();
builder.Services.AddSession(); // Make sure session is enabled
builder.Services.AddDistributedMemoryCache();

// Add controllers with views
builder.Services.AddControllersWithViews();

// Add authentication with cookies
builder.Services.AddAuthentication("MyCookieAuth") // scheme name
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // redirect when not authenticated
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

// Add authorization
builder.Services.AddAuthorization();


//Repos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IHelperService, Helper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Custom Middleware must come **after UseRouting**
app.UseSession();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

// This is critical — endpoint resolution happens here
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
