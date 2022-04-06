using AutoMapper;
using BookClub.DTO.Extensions;
using BookClub.DTO.Interface;
using BookClub.DTO.Repository;
using BookClub.Extensions;
using Microsoft.OpenApi.Models;

namespace BookClub
{
    public class Startup
    {      
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            services.AddTransient<IDataAccessRepository, DataAccessRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();

            services.AddLogicInjection<Startup>();

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BookClub",
                    Description = " Книжный клуб, в котором пользователи могут составлять список прочитанных книг.",
                    Contact = new OpenApiContact
                    {
                        Name = "Денисенко Марк",
                        Email = "Morearti1726263@yandex.ru"
                    },
                    Version = "v1"
                });
            });
           

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookClub v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
