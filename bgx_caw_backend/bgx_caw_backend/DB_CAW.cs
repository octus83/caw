using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgx_caw_backend
{
    /// <summary>
    /// partial class that implements main functions to connect to fixed database
    /// </summary>
    partial class DB_CAW : IDisposable
    {
        public SqlConnection sql_connection;
        public static SqlConnectionStringBuilder connection_string = new SqlConnectionStringBuilder
        {
            DataSource = "N005509\\trans_edb_p8",
            InitialCatalog = "CAW",
            IntegratedSecurity = true
        };

        /// <summary>
        /// calls SqlConnection.Open to open connection to database
        /// </summary>
        public DB_CAW()
        {
            sql_connection = new SqlConnection(connection_string.ConnectionString);
            sql_connection.Open();
        }

        public static void checkConnection()
        {
            SqlConnection test_connection = new SqlConnection(connection_string.ConnectionString);
            test_connection.Open();
            test_connection.Close();
        }

        /// <summary>
        /// public function (implemented with IDisposable-Interface) that closes the SqlConnection
        /// </summary>
        public void Dispose()
        {
            sql_connection.Close();
        }
    }

    /// <summary>
    /// partial class that implements additional functions for the caw_backend
    /// </summary>
    partial class DB_CAW
    {
        SqlCommand sql_cmd;

        public List<Diagramm> getDiagramms()
        {
            sql_cmd = new SqlCommand("SELECT * FROM dbo.tbldiagramm");
            SqlDataReader data_reader = sql_cmd.ExecuteReader();

            List<Diagramm> result_list = new List<Diagramm>();

            while (data_reader.Read())
            {
                result_list.Add(new Diagramm
                {
                    Author = data_reader["author"].ToString(),
                    Date_init = DateTime.Parse(data_reader["date_init"].ToString()),
                    Date_lastchange = DateTime.Parse(data_reader["date_lastchange"].ToString()),
                    DeviceID = data_reader["deviceID"].ToString(),
                    Fieldname = data_reader["fieldname"].ToString(),
                    ID = Int32.Parse(data_reader["id"].ToString()),
                    IsActive = Boolean.Parse(data_reader["isActive"].ToString()),
                    Projectname = data_reader["projectname"].ToString(),
                    Projectnumber = data_reader["projectnumber"].ToString(),
                    Productionplace = data_reader["productionplace"].ToString(),
                    Serialnumber = data_reader["serialnumber"].ToString(),
                    Worker = data_reader["worker"].ToString(),
                });
            }

            data_reader.Close();

            return result_list;
        }

        public void addDiagramm(Diagramm diagramm)
        {
            sql_cmd = new SqlCommand();
            sql_cmd.Connection = sql_connection;

            sql_cmd.Parameters.Add("@Author", SqlDbType.VarChar, 50).Value = diagramm.Author;
            sql_cmd.Parameters.Add("@Date_init", SqlDbType.DateTime).Value = diagramm.Date_init;
            sql_cmd.Parameters.Add("@Date_lastchange", SqlDbType.DateTime).Value = diagramm.Date_lastchange;
            //sql_cmd.Parameters.Add("@DeviceID", SqlDbType.VarChar, 10).Value = diagramm.DeviceID;
            sql_cmd.Parameters.Add("@FieldName", SqlDbType.VarChar, 50).Value = diagramm.Fieldname;
            sql_cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = diagramm.IsActive;
            sql_cmd.Parameters.Add("@ProjectName", SqlDbType.VarChar, 50).Value = diagramm.Projectname;
            sql_cmd.Parameters.Add("@ProjectNumber", SqlDbType.VarChar, 10).Value = diagramm.Projectnumber;
            sql_cmd.Parameters.Add("@ProductionPlace", SqlDbType.VarChar, 10).Value = diagramm.Productionplace;
            sql_cmd.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 10).Value = diagramm.Serialnumber;
            sql_cmd.Parameters.Add("@Worker", SqlDbType.VarChar, 10).Value = diagramm.Worker;

            try
            {
                sql_cmd.CommandText = "IF NOT EXISTS (SELECT * FROM dbo.tbldiagramm WHERE serialnumber = @SerialNumber) " +
                                        "INSERT into dbo.tbldiagramm (author, date_init, date_lastchange, fieldname, isActive, projectname, projectnumber, productionplace, serialnumber, worker) " +
                                        "VALUES (@Author, @Date_init, @Date_lastchange, @FieldName, @IsActive, @ProjectName, @ProjectNumber, @ProductionPlace, @SerialNumber, @Worker)";
                sql_cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}
