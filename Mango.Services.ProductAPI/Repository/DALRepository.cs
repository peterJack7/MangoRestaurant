using Microsoft.Data.SqlClient;
using Nancy.Json;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Mango.Services.ProductAPI.Repository
{
    public class DALRepository : IDALRepository
    {
        
        SqlDataReader rdr = null;
        public SqlCommand cmd;
        public SqlDataAdapter adptr;
        string strConnection = "";
        public SqlConnection con = new SqlConnection();
        private readonly IConfiguration configuration;
        private readonly SqlConnection sqlConnection = new SqlConnection();

        public DALRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string SqlConnection()
        {
            strConnection = configuration.GetConnectionString("DefaultConnection").ToString();
            return strConnection;
        }

        public void ConnectionOpen()
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.ConnectionString = SqlConnection();
                sqlConnection.Open();
            }
        }

        public string ConvertDataTableTojSonString(DataTable dataTable)
        {
            JavaScriptSerializer serializer =
              new JavaScriptSerializer();
            List<Dictionary<String, Object>> tableRows = new List<Dictionary<String, Object>>();

            Dictionary<String, Object> row;

            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<String, Object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                tableRows.Add(row);
            }
            return serializer.Serialize(tableRows);
        }



        public async Task<DataSet> getDataSetForSqlParam(string procedure, SqlParameter[] param = null)
        {
            DataSet ds = new DataSet();
            try
            {
                ConnectionOpen();
                adptr = new SqlDataAdapter(procedure, sqlConnection);
                adptr.SelectCommand.CommandTimeout = 0;
                adptr.SelectCommand.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter parm in param)
                {
                    adptr.SelectCommand.Parameters.Add(parm);
                }
                adptr.Fill(ds);

            }
            catch (Exception ex)
            {
                ds = null;
            }

            return ds;
        }
    }
}
