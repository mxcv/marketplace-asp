using System.Globalization;
using Marketplace.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ReleaseConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options => {
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireDigit = false;
	})
	.AddEntityFrameworkStores<MarketplaceDbContext>();

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
			IssuerSigningKey = jwtConfig.SymmetricSecurityKey,
			ClockSkew = TimeSpan.Zero
		};
	});
builder.Services.AddAuthorization(options =>
	options.DefaultPolicy =
		new AuthorizationPolicyBuilder(
			CookieAuthenticationDefaults.AuthenticationScheme,
			JwtBearerDefaults.AuthenticationScheme)
		.RequireAuthenticatedUser()
		.Build()
);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
	.AddDataAnnotationsLocalization()
	.AddViewLocalization();
//.AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

var supportedCultures = new[] {
	new CultureInfo("en"),
	new CultureInfo("ru")
};
app.UseRequestLocalization(new RequestLocalizationOptions {
	DefaultRequestCulture = new RequestCulture("en", "en"),
	SupportedCultures = supportedCultures,
	SupportedUICultures = supportedCultures
});

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
