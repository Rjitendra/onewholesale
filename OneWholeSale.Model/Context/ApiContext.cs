namespace OneWholeSale.Model.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Entity;
    using OneWholeSale.Model.Entity.FullFillCenter;
    using OneWholeSale.Model.Entity.SalesPerson;

    public class ApiContext : IdentityDbContext<ApplicationUser, ApplicationUserIdentityRole, int>
    {
        public ApiContext(string connectionString) : this(new DbContextOptionsBuilder<ApiContext>().UseSqlServer(connectionString).Options)
        {
            // This constructor is for support to use this context in LinqPad
        }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        public virtual DbSet<SalesPerson> SalesPerson { get; set; }
        public virtual DbSet<FullFillmentCenter> FullFillmentCenter { get; set; }
        public virtual DbSet<MapSalesPersonToFC> MapSalesPersonToFC { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}