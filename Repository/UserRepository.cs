

using JwtAuthentication.Models;
using JwtAuthentication.Services;

namespace JwtAuthentication.Repository;

public class UserRepository : IUserRepository
{
	private readonly UserSqlService _services;
	public UserRepository(IConfiguration configuration)
	{
		_services = new UserSqlService(configuration["ConnectionStrings:LocalConnection"]);
	}
	public async Task<User?> Login(User user)
	{
		return await _services.Select(user);
	}
}