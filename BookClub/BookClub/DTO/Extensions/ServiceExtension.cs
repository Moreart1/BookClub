using BookClub.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BookClub.DTO.Extensions
{
    public static class ServiceExtension
    {
        public static void AddLogicInjection<T>(this IServiceCollection services)
        {
            services.AddDbContext<DataBaseContext>(options => options.UseSqlServer("Server=(localdb)\\v12.0;Database=BookClub;Trusted_Connection=True;"), ServiceLifetime.Transient);
        }
    }
}
