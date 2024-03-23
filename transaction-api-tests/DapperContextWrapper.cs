using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction_api_tests.Interfaces;

namespace transaction_api_tests
{
    public class DapperContextWrapper : IDapperContext
    {
        private readonly DapperContext _DapperContext;

        public DapperContextWrapper(DapperContext dapperContext)
        {
            _DapperContext = dapperContext;
        }

        public IDbConnection CreateConnection()
        {
            return _DapperContext.CreateConnection();
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object parameters)
        {
            return _DapperContext.CreateConnection().QuerySingleOrDefaultAsync<T>(sql, parameters);
        }
    }
}
