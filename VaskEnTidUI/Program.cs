using VaskEnTidLib.Services;
using VaskEnTidLib.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace VaskEnTidUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LaundryManagementDB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Application Name=\"SQL Server Management Studio\";Command Timeout=30";

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddSingleton(new MachineRepo(_connectionString));
            builder.Services.AddSingleton<MachineService>();

            builder.Services.AddSingleton(new UserRepo(_connectionString));
            builder.Services.AddSingleton<UserService>();

            builder.Services.AddSingleton(new BookingRepo(_connectionString));
            builder.Services.AddSingleton<BookingService>();

            builder.Services.AddSingleton(new ErrorReportRepo(_connectionString));
            builder.Services.AddSingleton<ErrorReportService>();



            builder.Services.AddDistributedMemoryCache(); // Stores session in memory
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1); // Session expires after 1 hour
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
