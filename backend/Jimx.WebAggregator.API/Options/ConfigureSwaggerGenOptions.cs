using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Jimx.WebAggregator.API.Options;

public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
	public void Configure(string? name, SwaggerGenOptions options)
	{
		Configure(options);
	}

	public void Configure(SwaggerGenOptions options)
	{
		options.SwaggerDoc("v1", new OpenApiInfo { Title = "Jimx.WebAggregator.API", Version = "v1" });
		options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			In = ParameterLocation.Header,
			Description = "Please enter token",
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			BearerFormat = "JWT",
			Scheme = "bearer"
		});

		options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
		{
			[new OpenApiSecuritySchemeReference("Bearer", doc)] = []
		});
	}
}