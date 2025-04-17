using Microsoft.OpenApi.Models;
using Pliromi.API.Filters;
using Pliromi.API.Token;
using Pliromi.Application;
using Pliromi.Domain.Security.Tokens;
using Pliromi.Infrastructure;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));


builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
	{
		Description =
			@"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType
			.ApiKey,
		Scheme = AUTHENTICATION_TYPE
	});


	options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType
							.SecurityScheme,
						Id = AUTHENTICATION_TYPE
					},
					Scheme = "oauth2",
					Name = AUTHENTICATION_TYPE,
					In = ParameterLocation.Header
				},
				new List<string>()
			}
		}
	);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();


app.Run();

