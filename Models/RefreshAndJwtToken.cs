namespace JwtAuthentication.Models
{
	public class RefreshAndJwtToken
	{
		public RefreshToken? RefreshToken { get; set; }
		public string? Token { get; set; }

	}
}
