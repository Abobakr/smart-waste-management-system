using DomainDataEntities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SCADADataAccessLayer
{     
   public partial class SCADADbContext : DbContext 
    {
        public SCADADbContext()
            : base("name=SCADACloudDBEntities")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<UnitItem> UnitItems { get; set; }
        public DbSet<TerminalComponent> TerminalComponents { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActiveMode> ActiveModes { get; set; }
        public DbSet<TerminalItem> TerminalItems { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<AccountTerminal> AccountTerminals { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Account> Accounts { get; set; }  
   }
}
