using API.SSE;

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

            builder.Services.AddSingleton<ServerSentEventsService>();
            builder.Services.AddSingleton<ServerSentEventsManager>();

            builder.Services.AddCors(options => {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy => {
                        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().WithMethods("PUT", "DELETE", "GET", "POST"); ;
                    });
            });

            var app = builder.Build();

            app.UseCors(MyAllowSpecificOrigins);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}