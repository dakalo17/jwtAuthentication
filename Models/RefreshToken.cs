namespace JwtAuthentication.Models
{
	public class RefreshToken
	{
		public int FkUserId { get; internal set; }
		public string? RToken { get; internal set; }
		public DateTime ExpiringDate { get; internal set; }
		public string? Key { get; internal set; }
	}
}
