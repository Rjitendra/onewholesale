namespace OneWholeSale.Client
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using OneWholeSale.Client.Extentions;

    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            builder.Services.AddMvc();
            builder.Services.AddSession();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = "/Account/Login";
                            options.LogoutPath = "/Account/Logout";
                            options.AccessDeniedPath = "/Account/AccessDenied";
                            options.ExpireTimeSpan = TimeSpan.FromHours(8);
                            options.SlidingExpiration = true;
                        });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization(options =>
                       {
                           options.AddPolicy("AdminOnly", policy =>
                           {
                               policy.RequireRole("Admin");
                           });
                       });
            builder.Services.AddTransient<UnauthorizedRequestHandler>();
            return builder.Build();
        }
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
            return app;
        }
    }
}
