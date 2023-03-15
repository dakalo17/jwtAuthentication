namespace JwtAuthentication.Models
{
	public class RefreshToken
	{
		public int FkUserId { get; set; }
		public string? RToken { get; set; }
		public DateTime ExpiringDate { get; set; }
		public string? Key { get; set; }
	}
}
