using System.Text;
using System.Text.Json.Serialization;
using ApiGestaoClinicas.Data;
using ApiGestaoClinicas.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApiGestaoClinicas.Configuration
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                            {
                                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                            });

            return builder;
        }
    }
}
