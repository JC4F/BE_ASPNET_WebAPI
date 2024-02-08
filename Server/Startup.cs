using BusinessObject.Commons;
using BusinessObject.Models;
using DataAccess.AutoMappers;
using DataAccess.Interfaces;
using DataAccess.Services;
using EmailService;
using ImageService;
using ImageService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Server.Middlewares;
using System.Net.WebSockets;
using System.Text;

namespace Server
{
    public class Startup
    {
        private IConfiguration Configuration;
        private string corsName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options => {
                //options.SuppressModelStateInvalidFilter = true;
                options.InvalidModelStateResponseFactory = actionContext =>
                {

                    var errors = actionContext.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return new BadRequestObjectResult(new APIResponse<Object>
                    {
                        IsSuccess = false,
                        Message = string.Join(",", errors)
                    });
                };
            });
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(corsName, build => build.AllowAnyMethod()
                    .AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(hostName => true).Build());
            });

            services.AddAutoMapper(typeof(ApplicationMapper));

            InjectServices(services);
            ConfigureJWT(services);
        }


        // add servides

        private void InjectServices(IServiceCollection services)
        {
            services.AddLogging();
            // add services repository pattern
            services.AddDbContext<PRN231Context>();

            // repository
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<IBlogRatingRepository, BlogRatingRepository>();

            // email service
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // cloudinary
            var cloudinaryConfig = Configuration
                .GetSection("Cloudinary")
                .Get<CloudinaryEnvConfiguration>();
            services.AddSingleton(cloudinaryConfig);
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        private void ConfigureJWT(IServiceCollection services)
        {
            // For Identity
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<PRN231Context>()
            //    //.AddRoles<IdentityRole>()
            //    .AddDefaultTokenProviders();

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtSetting:ValidIssuer"],
                    ValidAudience = Configuration["JwtSetting:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSetting:Secret"]))
                };
                // Other configs...
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        var result = JsonConvert.SerializeObject(new { isSuccess = false, message = "UnAuthorization" });
                        context.Response.ContentType = "application/json;charset=utf-8";
                        // Call this to skip the default logic and avoid using the default response
                        context.HandleResponse();

                        // Write to the response in any way you wish
                        context.Response.StatusCode = 401;
                        //context.Response.Headers.Append("my-custom-header", "custom-value");
                        await context.Response.WriteAsync(result);
                    }
                };
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(corsName);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    string[] roleNames = { ROLE.ADMIN, ROLE.SELLER, ROLE.USER };

            //    foreach (var roleName in roleNames)
            //    {
            //        if (!await roleManager.RoleExistsAsync(roleName))
            //        {
            //            await roleManager.CreateAsync(new IdentityRole(roleName));
            //        }
            //    }
            //}

            app.Run();
        }
    }
}
