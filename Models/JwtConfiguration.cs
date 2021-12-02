using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Models
{
	public class JwtConfiguration
	{
		public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public string? Lifetime { get; set; }
		public string? Key { get; set; }

		public SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key ?? throw new NullReferenceException()));
		}
		public TimeSpan GetTimeSpanLifetime()
		{
			return TimeSpan.FromMinutes(double.Parse(Lifetime ?? throw new NullReferenceException()));
		}
	}
}
