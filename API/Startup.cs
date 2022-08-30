using API.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
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
            services.AddControllers();

            // Add the JWT service
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(
                jwtOptions =>
                {
                    // JwtConfiguration key in appsettings.json or appsettings.Development.json
                    var key = Configuration.GetValue<string>("JwtConfig:Key");
                    // Convert key to byte array
                    var keyBytes = Encoding.UTF8.GetBytes(key);

                    // Define the token validation options
                    jwtOptions.SaveToken = true;
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        // Overtime the token lifetime
                        // Default value is 5 minutes, set to 0 to disable it
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Add the interfaces and classes
            services.AddSingleton(typeof(IJwtTokenManager), typeof(JwtTokenManager));

            // Add the swagger service
            services.AddSwaggerGen();

            // Add the cors rules allowing any origin
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable the cors
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            // Enable swagger interface
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            // Enable autentication
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
