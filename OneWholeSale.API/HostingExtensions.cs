namespace OneWholeSale.API
{
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System.Reflection;
    using OneWholeSale.Model.Context;
    using OneWholeSale.API.Extentions;
    using OneWholeSale.API.Policies;
    using OneWholeSale.Model.Utility;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using OneWholeSale.API.Config;
    using Microsoft.AspNetCore.Identity;
    using OneWholeSale.Model.Entity;
    using OneWholeSale.API.Controllers;
    using System.Security.Claims;
    using OneWholeSale.Service.Utility;
    using OneWholeSale.Model.Enums;

    public static class HostingExtensions
    {
        private const string CorsPolicy = "_MyAllowSubdomainPolicy";
        private const string IdentityServerSettings = "IdentityServerSettings";

        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // builder.Services.AddApplicationInsightsTelemetry();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            var assembly = typeof(ApiContext).GetTypeInfo().Assembly.GetName().Name;
            builder.Services.AddDbContext<ApiContext>(builder =>
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("OneWholeSale.API")));

            // Add ASP.NET Core Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationUserIdentityRole>(IdentityServerConfigurations.GetConfigureIdentityOptions())
                .AddEntityFrameworkStores<ApiContext>()
                .AddDefaultTokenProviders();

            // configure authentication to use IdentityServer
            var identityServerSettings = builder.Configuration.GetSection("IdentityServerSettings").Get<IdentityServerSettings>();

            builder.Services.Configure<IdentityServerSettings>(builder.Configuration.GetSection(IdentityServerSettings));

            // add the configuration settings to the dependency injection container
            builder.Services.AddSingleton(identityServerSettings);

            builder.Services.AddHttpContextAccessor(); // Needed for HttpContextHelper

            builder.Services.ConfigureDIServices();

            // Add MVC support
            builder.Services.AddMvc(options => { options.EnableEndpointRouting = true; });



            builder.Services.AddCors(options =>
              options.AddPolicy(CorsPolicy, builder => builder.WithOrigins(identityServerSettings.AllowedOrigins).AllowAnyHeader().AllowAnyMethod()));

            // Add Authorization Policies
            builder.Services.ConfigureAuthorizationPolicies();


            // Adding Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = identityServerSettings.ValidAudience,
                    ValidIssuer = identityServerSettings.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityServerSettings.ApiSecret))
                };
            });

            // configure services for injection
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson();

            var defaultApiVersion = new ApiVersion(1, 0);
            var apiVersionReader = new HeaderApiVersionReader("api-version");
            builder.Services
                .AddMvcCore();

            // Add support for versioning in the API
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = defaultApiVersion;
                options.ApiVersionReader = apiVersionReader;
                options.ReportApiVersions = true;
            });

            // This is to have Swagger support multiple versions.

            // configure Global Application Settings
            //  PopulateGlobalSettings(builder.Services);
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddControllers();
            // Add Swagger services
            RegisterDocumentationGenerators(builder.Services);

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        });
            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                var virDir = app.Configuration.GetSection("VirtualDirectory");
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(virDir.Value + "/swagger/v1/swagger.json", "v1");
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseRouting();
            app.UseStaticFiles();
            app.ConfigureExceptionHandler();
            //  app.UseHttpsRedirection();
            app.UseCors(CorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureRedundantStatusCodePages(); // Provide JSON responses for standard response codes such as HTTP 401.
            app.UseHttpContextHelper(); // Helper to get Base URL anywhere in application
            InitializeRoles(app.Services).Wait();
            InitializeUser(app.Services).Wait();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            return app;
        }



        private static void RegisterDocumentationGenerators(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OneWholeSale.API",
                    Description = "An ASP.NET Core Web API for managing OneWholeSale.API items"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        private static async Task InitializeRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationUserIdentityRole>>();



            foreach (var roleName in Constants.RoleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationUserIdentityRole();
                    role.Name = roleName;
                    await roleManager.CreateAsync(role);
                }
            }
        }
        private static async Task InitializeUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var applicationUser = new ApplicationUser("jitendrabehera64@gmail.com");
            applicationUser.EmailConfirmed = true;
            var result = await userManager.CreateAsync(applicationUser, "Royalclassic64#");
            if (result.Succeeded)
            {
                // User account created successfully
                // add user claims
                var claims = new List<Claim>
                {
                        new Claim(IdentityClaims.FullName, "Jitendra Behera"),
                        new Claim(IdentityClaims.FirstName, "Jitendra"),
                        new Claim(IdentityClaims.LastName, "Behera"),
                        new Claim(IdentityClaims.Email, "jitendrabehera64@gmail.com"),
                };
                var user = await userManager.FindByEmailAsync("jitendrabehera64@gmail.com");
                await userManager.AddClaimsAsync(user, claims);
                await userManager.AddToRoleAsync(user, ApplicationUserRole.Admin.ToString());
            }
            else
            {
                // Handle errors
            }
        }
    }
}