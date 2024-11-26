using BC_Api.Interfaces;
using BC_Api.Models;
using BC_Api.Services;
using BC_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient<KCBService>();
builder.Services.AddHttpClient<TokenService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<SeminarService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseDefaultCredentials = true,
    });

builder.Services.AddHttpClient<CustomerService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseDefaultCredentials = true,
    });

builder.Services.AddHttpClient<EmployeeService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseDefaultCredentials = true,
    });
builder.Services.AddScoped<ISeminar, SeminarService>();
builder.Services.AddSingleton<Credentials>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
}).AddEntityFrameworkStores<AppDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        //ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
    };

});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();
//app.UseMiddleware<TokenAuthenticationMiddleware>();
app.MapWhen(
    context => context.Request.Path.StartsWithSegments("/api/Kcb", StringComparison.OrdinalIgnoreCase) ||
               context.Request.Path.StartsWithSegments("/api/Query", StringComparison.OrdinalIgnoreCase),
    builder =>
    {
        builder.UseMiddleware<TokenAuthenticationMiddleware>();

        // Ensure controllers for the specified routes are mapped
        builder.UseRouting();
        builder.UseCors("AllowReactApp");
        builder.UseHttpsRedirection();
        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    });

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();