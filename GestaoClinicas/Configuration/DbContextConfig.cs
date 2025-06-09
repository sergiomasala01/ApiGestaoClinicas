using System.Text.Json.Serialization;
using ApiGestaoClinicas.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiGestaoClinicas.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            return builder;
        }
    }
}
