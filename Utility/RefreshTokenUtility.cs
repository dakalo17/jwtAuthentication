using JwtAuthentication.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthentication.Utility
{
	public class RefreshTokenUtility
	{

		private readonly string? _secret;
		private readonly SymmetricSecurityKey _key;

		public RefreshTokenUtility(IConfiguration configuration)
		{
			_secret = configuration["Jwt:Secret"];
			
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret??string.Empty));
		}

		public RefreshAndJwtToken GenerateTokens(User? user, RefreshToken? refreshToken)
		{
			if (user is null) return null;
			var claims = new Dictionary<string, object>() {
				{ ClaimTypes.NameIdentifier,user.Id},
				{ ClaimTypes.Email,user.Email??string.Empty},
			};

			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
			var handler = new JwtSecurityTokenHandler();

			var token = handler.CreateJwtSecurityToken(
				new SecurityTokenDescriptor
				{
					Claims = claims,
					Expires = DateTime.UtcNow.AddSeconds(30),
					SigningCredentials = creds

				}
				);

			var obj = handler.WriteToken(token);



			//if refreshToken is null that means we need to generate one
			var jwtObj = new RefreshAndJwtToken
			{
				Token = obj,
				RefreshToken =
				refreshToken is null ?
				new RefreshToken
				{
					RToken = GenerateRefreshToken(),
					ExpiringDate = DateTime.UtcNow.AddDays(2),
					FkUserId = user.Id,
					Key = Guid.NewGuid().ToString()

				}
				: refreshToken


			};



			return jwtObj;
		}


		private string GenerateRefreshToken()
		{
			var randomBytes = new byte[32];

			using var rand = RandomNumberGenerator.Create();
			rand.GetBytes(randomBytes);
			return Convert.ToBase64String(randomBytes);
		}


		public User? GetUserFromToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();

			if (string.IsNullOrEmpty(token) || !handler.CanReadToken(token))
				return null;

			var param = GetTokenValidatorParams();

			
			
			var readToken = handler.ReadJwtToken(token);
			
			
			
			var claims = readToken.Claims;
			
			var userId = Convert.ToInt32(claims.FirstOrDefault(x => x.Type =="nameid")?.Value);

			var userEmail = claims.FirstOrDefault(x => x.Type == "email")?.Value;


			//principal = handler.ValidateToken(token,
			//param, out var validatedToken);


			//var userEmail = principal?.FindFirstValue(ClaimTypes.Email);
			//var userId = Convert.ToInt32(principal?.FindFirstValue(ClaimTypes.NameIdentifier));
			

			return new User
			{
				Id = userId,
				Email = userEmail
			};
		}


		private TokenValidationParameters GetTokenValidatorParams(bool expire = true)
		{
			return new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = _key,

				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = expire,

				RequireExpirationTime = expire,

				ClockSkew = TimeSpan.Zero


			};
		}
	}
}
