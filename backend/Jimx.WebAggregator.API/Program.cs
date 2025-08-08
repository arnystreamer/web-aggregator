using Jimx.WebAggregator.API.Options;
using Jimx.WebAggregator.API.Services;
using Jimx.WebAggregator.Calculations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer();
builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();

builder.Services.AddCors();
builder.Services.ConfigureOptions<ConfigureCorsOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.Configure<GeneralOptions>(
	builder.Configuration.GetSection(GeneralOptions.OptionName));

builder.Services.Configure<CitiesDatabaseSettings>(
	builder.Configuration.GetSection(CitiesDatabaseSettings.OptionName));

builder.Services.AddSingleton<SalaryTypesService>();
builder.Services.AddSingleton<CitiesDatabaseService>();
builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<TaxationService>();
builder.Services.AddSingleton<CrossRatesService>();
builder.Services.AddSingleton<SortingFunctionsService>();

builder.Services.AddSingleton<SettingsProvider>();
builder.Services.AddSingleton<KeysProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
