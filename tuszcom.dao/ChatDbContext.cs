using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using tuszcom.models.DAO;

namespace tuszcom.dao
{
    public class ChatDbContext: DbContext
    {
        protected static IConfigurationRoot config;
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
            config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErrorLogs>().ToTable("ErrorLogs");
            modelBuilder.Entity<ChatMessages>().ToTable("ChatMessages");
            modelBuilder.Entity<ChatMessageFiles>().ToTable("ChatMessageFiles");
            modelBuilder.Entity<ChatUserDetails>().ToTable("ChatUserDetails");
            modelBuilder.Entity<ViewMessages>().ToTable("ViewMessages");
            modelBuilder.Entity<AspNetUsers>().ToTable("AspNetUsers");
            modelBuilder.Entity<Settings>().ToTable("Settings");
        }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        public DbSet<ChatMessageFiles> ChatMessageFiles { get; set; }
        public DbSet<ChatUserDetails> ChatUserDetails { get; set; }
        public DbSet<ViewMessages> ViewMessages { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<Settings> Settings { get; set; }
    }
}
