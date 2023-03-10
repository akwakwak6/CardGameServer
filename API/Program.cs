using API.Infrastructure;
using API.SSE;
using BLL.Services;
using BLL.Services.Presi;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace API {
    public class Program {
        public static void Main(string[] args) {

            string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<PresiTableManagerBL>();
            builder.Services.AddSingleton<EventService>();

            builder.Services.AddCors(options => {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy => policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true)
                        .AllowCredentials());
            });


            builder.Services.AddScoped<CardGameDbContext>();
            builder.Services.AddScoped<TokenManager>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<PresiService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters() {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            builder.Configuration.GetSection("TokenInfo").GetSection("secret").Value))
                    };
                });

            builder.Services.AddAuthorization(option => {
                option.AddPolicy("admin", policy => policy.RequireRole("admin"));
                option.AddPolicy("user", policy => policy.RequireAuthenticatedUser());
            });

            var app = builder.Build();

            app.UseCors(MyAllowSpecificOrigins);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();

                //clean DB
                InitDB.CleanDB();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();


        }
    }
}