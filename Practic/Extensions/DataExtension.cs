using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Practic.Data.DbContexts;

namespace Practic.Extensions
{
    public static class DataExtension
    {
        /// <summary>
        /// Automatically updated database based on latest migration
        /// </summary>
        /// <param name="app"></param>
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.Migrate();
        }
    }
}
