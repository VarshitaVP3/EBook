using Database;
using Databases;
using Databases.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;
using Services;
using Services.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });



//builder.Services.AddDbContext<EbookDbContext>
//    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));

builder.Services.AddDbContext<EbookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("EBook")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<EbookDatabase>();  
builder.Services.AddScoped<AuthDatabase>();
builder.Services.AddScoped<IEbook, EbookDatabase>();
builder.Services.AddScoped<IEbook,EbookServices>();
builder.Services.AddScoped<IAuthService, AuthDatabase>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<EbookDbService>();
builder.Services.AddScoped<IDatabase, EbookDbService>();    

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.Configure<appsetting>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });

    // Define the security schema
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    // Apply security schema globally
    c.AddSecurityDefinition("Bearer", securityScheme);

    // Define the security requirement
    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };

    // Apply security requirement globally
    c.AddSecurityRequirement(securityRequirement);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

if (app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/error");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
