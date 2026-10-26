using BookClub.DAL.DBContext;
using BookClub.DAL.Interface;
using BookClub.DAL.Repository;
using BookClub.DTO.Interface;
using BookClub.DTO.Repository;
using BookClub.DTO.Service;
using Microsoft.EntityFrameworkCore;

namespace BookClub.DTO.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IDataAccessRepository, DataAccessRepository>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            return services;
        }
    }
}
