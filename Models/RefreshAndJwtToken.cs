namespace JwtAuthentication.Models
{
	public class RefreshAndJwtToken
	{
		public RefreshToken? RefreshToken { get; set; }
		//Access token
		public string? Token { get; set; }

	}
}
