using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Survey.Models;
using Survey.Persestance;
using Survey.Services;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Survey.Contracts.Auth;
using Survey.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using System.Runtime.CompilerServices;


namespace Survey
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services , IConfiguration configuration)
        {
            // Add services to the container.

            services.AddControllers();
            var conn = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection String 'DefaultConnection' not found.");
            services.AddDbContext<DBContext>(opttions => opttions.UseSqlServer(conn));
            services.
                AddSwaggerServices()
                .AddMappsterServices()
                .AddAuthServices(configuration)
                .AddCORSServices()
                .AddConfigServices();

            services.AddScoped<IPollServices, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IQuestionsServices, QuestionServices>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddExceptionHandler<GlobalExceptionHandeler>();
            services.AddProblemDetails();

            return services;
        }
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static IServiceCollection AddMappsterServices(this IServiceCollection services)
        {
            var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
            mappingConfiguration.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfiguration));
            return services;
        }
        public static IServiceCollection AddConfigServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddFluentValidationAutoValidation();
            return services;
        }
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtProvidor, JwtProvidor>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DBContext>();
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName).ValidateDataAnnotations().ValidateOnStart();
            var settings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
                )
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings!.Key)),
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience
                    };
                });

            return services;
        }
        public static IServiceCollection AddCORSServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
            builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyMethod()));
            return services;
        }
    }
}
