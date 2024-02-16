
using Microsoft.AspNetCore.Authentication.Cookies;
using ThaiRestaurant.Data;
//using ThaiRestaurant.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ImageUploadService>(); 
builder.Services.AddScoped<DatabaseContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opts => opts.LoginPath = "/User/Login");
builder.Services.AddScoped<ImageUploadService>();


builder.Logging.AddAWSProvider(builder.Configuration.GetAWSLoggingConfigSection());
builder.Logging.SetMinimumLevel(LogLevel.Debug);

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


//ip
//app.UseMiddleware<IpRestrictionMiddleware>();
//ip:
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
