using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Models
{
	public class JwtConfiguration
	{
		public string? Issuer { get; set; }
		public string? Audience { get; set; }
		public string? AccessLifetime { get; set; }
		public string? RefreshLifetime { get; set; }
		public string? Key { get; set; }

		public TimeSpan AccessTimeSpan =>
			LifetimeToTimeSpan(AccessLifetime ?? throw new NullReferenceException());
		public TimeSpan RefreshTimeSpan =>
			LifetimeToTimeSpan(RefreshLifetime ?? throw new NullReferenceException());
		public SymmetricSecurityKey SymmetricSecurityKey =>
			new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key ?? throw new NullReferenceException()));

		private TimeSpan LifetimeToTimeSpan(string lifetime)
		{
			return TimeSpan.FromSeconds(double.Parse(lifetime));
		}
	}
}
