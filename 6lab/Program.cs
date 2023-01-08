using System.Text.Json.Serialization;

namespace _6lab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddMvcCore(options =>
            //{
            //    options.RequireHttpsPermanent = true; // does not affect api requests
            //    options.RespectBrowserAcceptHeader = true; // false by default
            //                                               //options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();

            //})
            //    .AddFormatterMappings();
            // Add services to the container.
            builder.Services.AddDbContext<SewingCompanyContext>();

            builder.Services.AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                //o.JsonSerializerOptions.MaxDepth = 0;
            })
            ;

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}