using ProductMS;
using ProductMS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductMS.JwTFeatures;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<DatabaseContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));



builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DatabaseContext>();

/*builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 7;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
}

)
    .AddEntityFrameworkStores<DatabaseContext>();*/

var jwtSettings = builder.Configuration.GetSection("JwTSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyAdminUsers",
                    policy => policy.RequireRole("Admin"));
});


builder.Services.AddSingleton<JwtHandler>();

builder.Services.AddControllers();

 

var app = builder.Build();


app.UseHttpsRedirection();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
