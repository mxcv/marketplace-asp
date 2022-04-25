using System.Globalization;
using System.Security.Principal;
using Marketplace.Models;
using Marketplace.Repositories;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var jwtConfigSection = builder.Configuration.GetSection("JwtConfiguration");
var jwtConfig = jwtConfigSection.Get<JwtConfiguration>();

// Add services to the container.
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ReleaseConnection"))
);

builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IPrincipal>(provider =>
	provider.GetService<IHttpContextAccessor>()!.HttpContext!.User
);

builder.Services.Configure<JwtConfiguration>(jwtConfigSection);
builder.Services.AddOptions();

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
	.AddCertificate();

builder.Services.AddIdentity<User, IdentityRole<int>>(options => {
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireDigit = false;
	})
	.AddEntityFrameworkStores<MarketplaceDbContext>();

builder.Services.AddAuthentication(config => {
		config.DefaultScheme = "IdentityOrJwt";
	})
	.AddPolicyScheme("IdentityOrJwt", "Identity or JWT", options => {
		options.ForwardDefaultSelector = context => {
			var isBearerAuthorization = context
				.Request
				.Headers["Authorization"]
				.FirstOrDefault()?
				.StartsWith("Bearer ")
				?? false;
			var isApiPath = context
				.Request
				.Path
				.StartsWithSegments("/api", StringComparison.InvariantCulture);

			return isBearerAuthorization || isApiPath
				? JwtBearerDefaults.AuthenticationScheme
				: IdentityConstants.ApplicationScheme;
		};
	})
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
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
			IdentityConstants.ApplicationScheme,
			JwtBearerDefaults.AuthenticationScheme
		)
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
