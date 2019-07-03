using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeApi.Models;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeApi
{
    public class Startup
    {
        private ConnectionString writeConnectionString =
            new ConnectionString("exchanges.db")
            {
                Mode = FileMode.Exclusive
            };
        
        private string collectionName = "exchanges";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            InitializeDatabase();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
        
        private void InitializeDatabase()
        {
            using (var db = new LiteDatabase(writeConnectionString))
            {
                if (db.CollectionExists(collectionName)) return;
                
                var exchanges = db.GetCollection<Exchange>(collectionName);
                exchanges.InsertBulk(new[]
                {
                    new Exchange
                    {
                        ApiKey = Guid.NewGuid(),
                        Balance = 100,
                        Rates = new Dictionary<string, double>
                        {
                            ["RUB"] = 60,
                            ["EUR"] = 1.25,
                            ["USD"] = 1
                        }
                    },
                    new Exchange
                    {
                        ApiKey = Guid.NewGuid(),
                        Balance = 75,
                        Rates = new Dictionary<string, double>
                        {
                            ["RUB"] = 65,
                            ["EUR"] = 1.20,
                            ["USD"] = 1
                        }
                    },
                    new Exchange
                    {
                        ApiKey = Guid.NewGuid(),
                        Balance = 120,
                        Rates = new Dictionary<string, double>
                        {
                            ["RUB"] = 59,
                            ["EUR"] = 1.24,
                            ["USD"] = 1
                        }
                    }
                });
            }
        }

    }
}