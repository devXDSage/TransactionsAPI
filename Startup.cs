using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TransactionAPIApplication.Filters;
using TransactionAPIApplication.Models;

namespace TransactionAPIApplication
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                        .WithOrigins("http://localhost:8080")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ExampleFilter>();
            });

            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });



            string connectionstring = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                var builtInFactory = options.InvalidModelStateResponseFactory;
             //  options.SuppressModelStateInvalidFilter = true;

                options.InvalidModelStateResponseFactory = context =>
                {
                   
                    var logger = context.HttpContext.RequestServices
                                        .GetRequiredService<ILogger<Program>>();

                    logger.LogInformation("Automatic 400");

                    foreach (var modelState in context.ModelState)
                    {
                       foreach (var error in modelState.Value.Errors)
                        {
                            logger.LogInformation(error.ErrorMessage.ToString());
                        }
                    }
                    return builtInFactory(context);
                };
            });
            //services.AddDbContext<AppDBContext>(options => options.UseSqlServer(
            //            connectionstring
            //    ));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            AWSOptions awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>(awsOptions);

            services.AddAWSService<IAmazonDynamoDB>();
            services.AddSingleton<ExampleFilter>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            // Register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // middleware
            }                      
            else
            {
                app.UseHsts();
            }

            //app.Run(async (context) =>             // middleware
            //{
            //    await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //});

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
