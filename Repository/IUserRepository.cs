

using JwtAuthentication.Models;

namespace JwtAuthentication.Repository;

public interface IUserRepository
{
	Task<User?> Login(User user);
}