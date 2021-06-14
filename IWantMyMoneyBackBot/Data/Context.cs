using Microsoft.EntityFrameworkCore;

namespace IWantMyMoneyBackBot.Data
{
    public sealed class Context : DbContext
    {
        /// <summary>
        /// Данные займов.
        /// </summary>
        public DbSet<Debt> Debts { get; set; }
        
        /// <summary>
        /// Данные для подключения к Telegram.
        /// </summary>
        public DbSet<TelegramData> TelegramData { get; set; }
        
        /// <summary>
        /// Данные о пользователях.
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        public Context()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Debts.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<User>().HasKey(u => new {u.TelegramId, u.Name});
            // modelBuilder.Entity<Debt>().HasKey(d => new {d.DebtorId, d.LenderId});
        }
    }
}