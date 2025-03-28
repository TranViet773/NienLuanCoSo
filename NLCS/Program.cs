using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NLCS.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLCS.Areas.Admin.Repositories;
using NLCS.Repositories;
using NLCS.Areas.Dean.Services;
using NLCS.Areas.Dean.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Thêm dịch vụ cho MVC với Runtime Compilation
builder.Services.AddControllersWithViews()
       .AddRazorRuntimeCompilation();

//Config Session Start
builder.Services.AddDistributedMemoryCache(); // Cung cấp bộ nhớ cache phân phối
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true; // Giới hạn quyền truy cập cookie chỉ qua HTTP
    options.Cookie.IsEssential = true; // Cookie là cần thiết cho ứng dụng
});
//Config Session End

// Add services to the container.
builder.Services.AddControllersWithViews();
// Đăng ký dịch vụ IPdfService
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddDbContext<ApplicationDbContext>(option => 
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/LogInUser"; // path của trang login
    options.AccessDeniedPath = "/AccessDenied"; // đnag nhập rồi mà chưa có quyền thì chuyển tới Accessdenied 

});
builder.Services.AddHttpContextAccessor();// để use User trong service
builder.Services.AddScoped<DeanRepository>();
builder.Services.AddScoped<ManagerRepository>();
builder.Services.AddScoped<AccountRepository>();
//Them TrainingProgramService
builder.Services.AddScoped<ITrainingProgramService, TrainingProgramService>();
builder.Services.AddTransient<PdfService>();
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
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:Admin}/{controller=Home}/{action=Index}/{id?}");

app.Run();
