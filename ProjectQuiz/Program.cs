using ProjectQuiz.DAO;
using ProjectQuiz.Data;
using ProjectQuiz.Services;

namespace ProjectQuiz;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthentication("Login").AddCookie("Login", options =>
        {
            options.Cookie.Name = "Login";
            options.LoginPath = "/User/Login";
            options.AccessDeniedPath = "/User/Denied";
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role","Admin"));
            options.AddPolicy("ActiveUserOnly", policy => policy.RequireClaim("Status", "Active"));
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AdminContext>();
        builder.Services.AddTransient<UserDAO>();
        builder.Services.AddTransient<FeedbackDAO>();
        builder.Services.AddTransient<QuizDAO>();
        builder.Services.AddTransient<AdminDAO>();
        builder.Services.AddTransient<AdminService>();

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
