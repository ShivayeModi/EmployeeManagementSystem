using EmployeesApi.Models;
using EmployeesApi.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeesApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditLogs();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            PasswordHash.CreatePasswordHash("Password", out var hash, out var salt);
            // Seed test user data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Test User",
                    Email = "test@example.com",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        private void AddAuditLogs()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .ToList();

            foreach (var entry in modifiedEntries)
            {
                var entityName = entry.Entity.GetType().Name;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());

                var changes = new List<Dictionary<string, string>>();

                foreach (var property in entry.OriginalValues.Properties)
                {
                    var originalValue = entry.OriginalValues[property]?.ToString();
                    var currentValue = entry.CurrentValues[property]?.ToString();

                    if (originalValue != currentValue)
                    {
                        var change = new Dictionary<string, string>
                        {
                             { "Property", property.Name },
                             { "OldValue", originalValue },
                             { "NewValue", currentValue }
                        };
                        changes.Add(change);
                    }
                }

                if (changes.Count > 0)
                {
                    var auditLog = new AuditLog
                    {
                        EntityName = entityName,
                        EntityID = (int)primaryKey.CurrentValue,
                        Action = "Update",
                        ChangedData = JsonConvert.SerializeObject(changes),
                        Timestamp = DateTime.UtcNow,
                        UserID = 1 // Replace with logic to capture the actual user ID
                    };

                    AuditLogs.Add(auditLog);
                }
            }
        }

    }
}
