namespace OneWholeSale.Model.Context
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Entity;
    using OneWholeSale.Model.Entity.FullFillCenter;
    using OneWholeSale.Model.Entity.Kirana;
    using OneWholeSale.Model.Entity.Master;
    using OneWholeSale.Model.Entity.Partner;
    using OneWholeSale.Model.Entity.SalesPerson;
    using System.Collections.Generic;

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
        public virtual DbSet<District> District { get; set; }

        public virtual DbSet<Vw_SalesPerson> Vw_SalesPerson { get; set; }
        public virtual DbSet<MapDistrictToFc> MapDistrictToFc { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<Kirana> Kirana { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}