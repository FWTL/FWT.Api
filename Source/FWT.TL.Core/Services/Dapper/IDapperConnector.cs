using System;
using System.Data;
using System.Threading.Tasks;

namespace FWT.TL.Core.Services.Dapper
{
    public interface IDapperConnector
    {
        Task<T> Execute<T>(Func<IDbConnection, Task<T>> data);

        Task Execute(Func<IDbConnection, Task> data);
    }
}
