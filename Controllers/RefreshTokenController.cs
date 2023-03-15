using JwtAuthentication.Models;
using JwtAuthentication.Repository;
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
		public RefreshTokenController(IRefreshTokenRepository repository)
		{
			_repository = repository;
		}

		//gets called in order to refresh an expired token

		[HttpGet("Refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshAndJwtToken refreshAndJwt)
		{


			//await _repository.GetRefreshToken();

			return Ok();
		}





	}
}
