using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterInvoice.Services
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
