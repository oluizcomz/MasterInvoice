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
                serviceScope.ServiceProvider.GetService<ContextBase>().Database.Migrate();
            }
        }
    }
}
