using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models
{
	public class User : IdentityUser<int>
	{
		public int? CityId { get; set; }

		public string? Name { get; set; }
		public DateTimeOffset Created { get; set; }

		public virtual City? City { get; set; }
		public virtual UserImage? Image { get; set; }
		public virtual RefreshToken? RefreshToken { get; set; }
		public virtual ICollection<Item> Items { get; set; } = null!;
		[InverseProperty(nameof(Feedback.Reviewer))]
		public virtual ICollection<Feedback> LeftFeedback { get; set; } = null!;
		[InverseProperty(nameof(Feedback.Seller))]
		public virtual ICollection<Feedback> ReceivedFeedback { get; set; } = null!;
	}
}
