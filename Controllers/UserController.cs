using JwtAuthentication.Models;
using JwtAuthentication.Repository;
using JwtAuthentication.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserRepository _repository;
		private readonly RefreshTokenUtility _utility;

		private readonly IRefreshTokenRepository _refreshTokenRepository;
		public UserController(IUserRepository repository,IRefreshTokenRepository refreshTokenRepository,IConfiguration configuration)
		{
			_repository = repository;
			_refreshTokenRepository = refreshTokenRepository;
			_utility = new RefreshTokenUtility(configuration);
		}

		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody]User user)
		{
			var getUser = await _repository.Login(user);

			if (getUser is null) return NotFound("User not found");


			var generatedTokes = _utility.GenerateTokens(getUser, null);
			var res = await _refreshTokenRepository.PostResfreshToken(generatedTokes.RefreshToken);


			return res is not null ? Ok(generatedTokes) : BadRequest() ;
		}
	}
}
