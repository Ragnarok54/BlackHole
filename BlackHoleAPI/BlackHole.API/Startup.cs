using BlackHole.DataAccess;
using BlackHole.DataAccess.Repositories;
using BlackHole.Business.Services;
using BlackHole.Common;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using BlackHole.API.Hubs;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using BlackHole.API.Authorization;
using System.Linq;

namespace BlackHole.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings.SetConfig(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BlackHole API",
                    Version = Settings.Version,
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddDbContext<BlackHoleContext>(options => options.UseSqlServer(Settings.DatabaseConnectionString));

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod()
                                                                                          .AllowAnyHeader()
                                                                                          .WithOrigins(Settings.CorsOrigins.ToArray())
                                                                                          .AllowCredentials()));

            // UnitOfWork and Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>(_ => new UnitOfWork(Settings.DatabaseConnectionString, Configuration));
            services.AddScoped<IRepository<Attachment>, Repository<Attachment>>();
            services.AddScoped<IRepository<AttachmentType>, Repository<AttachmentType>>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IRepository<UserConversation>, Repository<UserConversation>>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<UserService>();
            services.AddScoped<ConversationService>();
            services.AddScoped<MessageService>();

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserProvider>();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "BlackHole API " + Settings.Version);

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                options.RoutePrefix = string.Empty;

                options.InjectStylesheet("/swagger-ui/custom.css");
            });
            app.UseStaticFiles();


            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/Messages/Hub");
                endpoints.MapHub<StatusHub>("/Status/Hub");
            });
        }
    }
}
