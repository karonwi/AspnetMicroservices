using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //MassTransit-RabbitMq Configuration
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((cfx, cfg) =>
                {
                    //the connection string of the rabbitmq
                    cfg.Host(Configuration["HostAddress"]);
                });
            });
            
            services.AddScoped<IBasketRepository, BasketRepository>();

            //the below is the IDIstributed cache distributed
            //Redis configuration
            services.AddStackExchangeRedisCache(options =>
            {
                //default host for redis    
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            //Grpc configuration
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (x => x.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

            //General configuration
            services.AddScoped<DiscountGrpcService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
