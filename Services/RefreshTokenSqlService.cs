using JwtAuthentication.Models;
using Npgsql;
using System.Threading.Tasks;
using System;

namespace JwtAuthentication.Services
{
	public class RefreshTokenSqlService : BaseSqlServices
	{
		public RefreshTokenSqlService(string? connectionString) : base(connectionString)
		{
		}

		public async Task<List<RefreshToken?>?> Select()
		{
			const string sql = "select * from \"refresh_token\"";

			using var cmd = new NpgsqlCommand(sql, _connection);

			List<RefreshToken?>? refreshTokens = null;
			try
			{
				await _connection.OpenAsync();
				await using var reader = await cmd.ExecuteReaderAsync();

				refreshTokens = new List<RefreshToken?>();

				while (await reader.ReadAsync())
				{
					if (reader.HasRows)
					{
						refreshTokens.Add(new RefreshToken
						{
							FkUserId = Convert.ToInt32(reader["fk_user_id"]),
							RToken = reader["token"].ToString() ?? string.Empty,
							ExpiringDate = Convert.ToDateTime(reader["expiring_date"]),
							Key = reader["key"].ToString() ?? string.Empty

						});
					}
				}
			}
			catch (Exception ex)
			{
				ex.GetBaseException();
				return null;
			}
			finally
			{
				await _connection.CloseAsync();
			}

			return refreshTokens;
		}

		public async Task<RefreshToken?> Select(int userId, string key)
		{
			const string sql = "select * from \"refresh_token\" " +
				"where fk_user_id = @fk_user_id and key=@key and expiring_date " +
				"between @expiring_date::timestamp and " +
				"@expiring_date::timestamp + interval '2 day' ";

			using var cmd = new NpgsqlCommand(sql, _connection);

			RefreshToken? refreshToken = null;
			try
			{
				cmd.Parameters.AddWithValue("@fk_user_id", userId);
				cmd.Parameters.AddWithValue("@key", key);
				cmd.Parameters.AddWithValue("@expiring_date", NpgsqlTypes.NpgsqlDbType.TimestampTz, DateTime.UtcNow);


				await _connection.OpenAsync();
				await using var reader = await cmd.ExecuteReaderAsync();


			
				while (await reader.ReadAsync())
				{
					if (reader.HasRows)
					{
						refreshToken = (new RefreshToken
						{
							FkUserId = Convert.ToInt32(reader["fk_user_id"]),
							RToken = reader["token"].ToString() ?? string.Empty,
							ExpiringDate = Convert.ToDateTime(reader["expiring_date"]),
							Key = reader["key"].ToString() ?? string.Empty


						});
					}
				}
			}
			catch (Exception ex)
			{
				ex.GetBaseException();
				return null;
			}
			finally
			{
				await _connection.CloseAsync();
			}

			return refreshToken;
		}


		public async Task<int?> Insert(RefreshToken? token)
		{
			const string sql = "insert into \"refresh_token\"(fk_user_id,token,key,expiring_date) " +
				"values(@fk_user_id,@token,@key,@expiring_date)";

			var rowsAffected = 0;
			try
			{
				await _connection.OpenAsync();
				using (var cmd = new NpgsqlCommand(sql, _connection))
				{
					cmd.Parameters.AddWithValue("@fk_user_id", token?.FkUserId ?? 0);
					cmd.Parameters.AddWithValue("@token", token?.RToken ?? string.Empty);
					cmd.Parameters.AddWithValue("@key", token?.Key ?? string.Empty);
					cmd.Parameters.AddWithValue("@expiring_date", token?.ExpiringDate ?? DateTime.UtcNow);


					rowsAffected = await cmd.ExecuteNonQueryAsync();
				}
			}
			catch (Exception ex)
			{
				ex.GetBaseException();
				return null;
			}
			finally
			{
				await _connection.CloseAsync();
			}

			return rowsAffected;
		}
	}
}
