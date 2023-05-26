namespace OneWholeSale.API.Extentions
{
    using OneWholeSale.Service.Implementations;
    using OneWholeSale.Service.Implementations.Interfaces;
    using OneWholeSale.Service.Interfaces;
    public static class ServiceExtensions
    {
        public static void ConfigureDIServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISalesPersonService, SalesPersonService>();
            services.AddTransient<Ipartner, PartnerService>();
            services.AddTransient<IKirana, KiranaService>();
        }
    }
}
