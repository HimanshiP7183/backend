using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Collabzone.Models
{
    public partial class CollabZoneContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // Constructor that accepts IConfiguration
        public CollabZoneContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // DbSets representing the tables in your database
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupMember> GroupMembers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        // Configuring the DbContext to use the connection string from appsettings.json
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("CollabZoneConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // Configuring models and relationships in the OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupId).HasColumnName("groupId");
                entity.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("createdAt").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.GroupName).HasMaxLength(255).IsUnicode(false).HasColumnName("groupName");
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime").HasColumnName("updatedAt").HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Groups).HasForeignKey(d => d.CreatedBy);
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.Property(e => e.GroupMemberId).ValueGeneratedNever().HasColumnName("groupMemberId");
                entity.Property(e => e.GroupId).HasColumnName("groupId");
                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
                entity.Property(e => e.JoinedAt).HasColumnType("datetime").HasColumnName("joinedAt").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Group).WithMany(p => p.GroupMembers).HasForeignKey(d => d.GroupId);
                entity.HasOne(d => d.User).WithMany(p => p.GroupMembers).HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.AdharCard).IsUnique();
                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.AdharCard).HasMaxLength(255).IsUnicode(false).HasColumnName("adharCard");
                entity.Property(e => e.Email).HasMaxLength(255).IsUnicode(false).HasColumnName("email");
                entity.Property(e => e.Name).HasMaxLength(255).IsUnicode(false).HasColumnName("name");
                entity.Property(e => e.Password).HasMaxLength(255).IsUnicode(false).HasColumnName("password");
            });
        }
    }
}
