using Npgsql;

namespace JwtAuthentication.Services
{
	public abstract class BaseSqlServices
	{
		protected readonly NpgsqlConnection _connection;
		public BaseSqlServices(string? connectionString) {
			_connection = new NpgsqlConnection(connectionString);
		}
	}
}
