using Microsoft.Data.SqlClient;
using System.Data;

namespace Mango.Services.ProductAPI.Repository
{
    public interface IDALRepository
    {
        DataSet getDataSetForSqlParam(string procedure, SqlParameter[] param = null);
        public String ConvertDataTableTojSonString(DataTable dataTable);
    }
}
