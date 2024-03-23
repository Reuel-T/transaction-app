using System.Data;

namespace transaction_api.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
        Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
