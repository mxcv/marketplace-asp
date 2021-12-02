using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Marketplace.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MarketplaceContext>(options =>
	options.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(opts => {
		opts.Password.RequireNonAlphanumeric = false;
		opts.Password.RequireLowercase = false;
		opts.Password.RequireUppercase = false;
		opts.Password.RequireDigit = false;
	})
	.AddEntityFrameworkStores<MarketplaceContext>();

var jwtConfigSection = builder.Configuration.GetSection(nameof(JwtConfiguration));
var jwtConfig = jwtConfigSection.Get<JwtConfiguration>();

builder.Services.Configure<JwtConfiguration>(jwtConfigSection);
builder.Services.AddOptions();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => {
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = new TokenValidationParameters {
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtConfig.Issuer,
			ValidAudience = jwtConfig.Audience,
			IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey(),
			ClockSkew = TimeSpan.Zero
		};
	});
builder.Services.AddAuthorization(options => {
	options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
		.RequireAuthenticatedUser()
		.Build();
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
