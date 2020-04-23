using AutoMapper;
using Kanbersky.Queue.Core.Extensions;
using Kanbersky.Queue.Core.Messaging.Abstract;
using Kanbersky.Queue.Core.Messaging.Concrete;
using Kanbersky.Queue.Core.Settings;
using Kanbersky.Queue.DAL.Concrete.EntityFramework.Context;
using Kanbersky.Queue.DAL.Concrete.EntityFramework.GenericRepository;
using Kanbersky.Queue.Service.Abstract;
using Kanbersky.Queue.Service.Concrete;
using Kanbersky.Queue.Service.Mappings.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kanbersky.Queue.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<ElasticSearchSettings>(_configuration.GetSection("ElasticSearchSettings"));

            services.AddDbContext<KanberContext>(opt =>
            {
                opt.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BusinessProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped(typeof(IExchangeFactory<>), typeof(ExchangeFactory<>));
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Kanbersky.Queue Microservice",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kanbersky Queue v1");
            });
        }
    }
}
