using System.Data.Common;

namespace JwtAuthentication.Models
{
	public class RefreshTokenIdentifier
	{
		public int UserId { get; set; }
		public string Key { get; set; } = string.Empty;
	}
}
