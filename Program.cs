using Microsoft.EntityFrameworkCore;
using Todo_list.Data;
using Todo_list.Services;
using Todo_list.Repositories;

namespace Todo_list
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            // Cấu hình Identity duy nhất và chuẩn
            /*builder.Services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();*/

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();


            // Đăng ký Service & Repository
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddHttpContextAccessor();



            var app = builder.Build();



            // Middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

    

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TodoTasks}/{action=Index}/{id?}");
            //app.MapRazorPages(); // Đảm bảo có dòng này để trang Login hoạt động

            app.Run();

            //Map API
            //app.MapControllers();

            //app.Run();
        }
    }
}
