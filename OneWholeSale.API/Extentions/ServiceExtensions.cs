namespace OneWholeSale.API.Extentions
{
    using OneWholeSale.Service.Implementations;
    using OneWholeSale.Service.Interfaces;
    public static class ServiceExtensions
    {
        public static void ConfigureDIServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ISalesPersonService, SalesPersonService>();
        }
    }
}
