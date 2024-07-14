using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Persistence
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialisation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ContextBase>();

                // Verificar se o banco de dados existe
                if (!context.Database.EnsureCreated())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}