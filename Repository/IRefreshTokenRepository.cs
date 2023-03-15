using JwtAuthentication.Models;

namespace JwtAuthentication.Repository
{
	public interface IRefreshTokenRepository
	{
		Task<RefreshToken?> GetRefreshToken(int userId,string key);
		Task<int?> PostResfreshToken(RefreshToken? refreshToken);

	}
}
