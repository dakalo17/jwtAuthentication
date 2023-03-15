using JwtAuthentication.Models;
using JwtAuthentication.Services;

namespace JwtAuthentication.Repository
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly RefreshTokenSqlService _services;

		public RefreshTokenRepository(IConfiguration configuration)
		{
			_services = new RefreshTokenSqlService(configuration["ConnectionStrings:LocalConnection"]);
		}

		public async Task<RefreshToken?> GetRefreshToken(int userId, string key)
		{
			return await _services.Select(userId, key);
		}

		public async Task<int?> PostResfreshToken(RefreshToken? refreshToken)
		{
			return await _services.Insert(refreshToken);
		}
	}
}
