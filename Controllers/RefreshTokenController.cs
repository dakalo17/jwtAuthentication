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
	public class RefreshTokenController : ControllerBase
	{

		private readonly IRefreshTokenRepository _repository;
		private readonly RefreshTokenUtility _utility;

		public RefreshTokenController(IRefreshTokenRepository repository,IConfiguration configuration)
		{
			_repository = repository;
			_utility = new RefreshTokenUtility(configuration);
		}

		//gets called in order to refresh an expired token

		[AllowAnonymous]
		[HttpPost("Refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshToken)
		{
			

			var refreshtoken = await _repository.GetRefreshToken(refreshToken.FkUserId, refreshToken.Key??string.Empty);
			if (refreshtoken is null || refreshtoken.RToken != refreshToken.RToken) 
				return NotFound("User's refresh token not found");

			var auth = Request.Headers["Authorization"];
			var token = auth[0]?.Split(" ")[1];


			//get user's details from expired access token
			var user = _utility.GetUserFromToken(token ?? string.Empty);

			//generate a new access token
			var res = _utility.GenerateTokens(user,refreshtoken);

			return res is not null ? Ok(res) : BadRequest("Could not generate token");
		}

		[HttpGet("GetRefreshToken")]
		public async Task<IActionResult> GetUserFromToken()
		{
			var auth = Request.Headers["Authorization"];
			var token = auth[0];


			var user = _utility.GetUserFromToken(token??string.Empty);

			
			
			return (user is not null && user.Id > 0) ?
				Ok(user.Id) : NotFound("user not found");
		}



	}
}
