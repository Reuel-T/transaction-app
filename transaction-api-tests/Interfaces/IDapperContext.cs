using System.Data;


namespace transaction_api_tests.Interfaces
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters);    
    }
}
