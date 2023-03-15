using JwtAuthentication.Models;
using Npgsql;

namespace JwtAuthentication.Services
{
	public class UserSqlService:BaseSqlServices
	{

		

		public UserSqlService(string? connectionString) : base(connectionString){
			
		}


		public async Task<bool?> Select(User userIn)
		{
			const string sql = "select * from \"user\" " +
			"where email=@email and password=@password;";


			User? user = null;
			try
			{
				await _connection.OpenAsync();

				using var cmd = new NpgsqlCommand(sql, _connection);


				cmd.Parameters.AddWithValue("@email", userIn.Email ?? string.Empty);
				cmd.Parameters.AddWithValue("@password", userIn.Password ?? string.Empty);

				using var reader = await cmd.ExecuteReaderAsync();

				while (await reader.ReadAsync())
				{
					if (reader.HasRows)
					{
						return reader["email"].ToString()?.ToLower()
							.Equals(userIn?.Email?.ToLower());
							
					}
				}

				if (!reader.IsClosed)
					await reader.CloseAsync();
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

			return false;
		}

	}
}
