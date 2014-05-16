using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

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
            InitialCatalog = "CAWFinal",
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
                    //Author = data_reader["author"].ToString(),
                    Date_init = DateTime.Parse(data_reader["date_init"].ToString()),
                    Date_lastchange = DateTime.Parse(data_reader["date_lastchange"].ToString()),
                    //DeviceID = data_reader["deviceID"].ToString(),
                    Fieldname = data_reader["fieldname"].ToString(),
                    //ID = Int32.Parse(data_reader["id"].ToString()),
                    IsActive = Boolean.Parse(data_reader["isActive"].ToString()),
                    Projectname = data_reader["projectname"].ToString(),
                    Projectnumber = data_reader["projectnumber"].ToString(),
                    Productionplace = data_reader["productionplace"].ToString(),
                    Serialnumber = data_reader["serialnumber"].ToString(),
                    //Worker = data_reader["worker"].ToString(),
                });
            }

            data_reader.Close();

            return result_list;
        }

        public Diagramm getDiagramm2(String serialNumber)
        {
            sql_cmd = new SqlCommand("SELECT * FROM dbo.tbldiagramm WHERE SerialNumber = " + serialNumber);
            SqlDataReader data_reader = sql_cmd.ExecuteReader();

            Diagramm result;

            while (data_reader.Read())
            {
                result = new Diagramm
                {
                    //Author = data_reader["author"].ToString(),
                    Date_init = DateTime.Parse(data_reader["date_init"].ToString()),
                    Date_lastchange = DateTime.Parse(data_reader["date_lastchange"].ToString()),
                    //DeviceID = data_reader["deviceID"].ToString(),
                    Fieldname = data_reader["fieldname"].ToString(),
                    //ID = Int32.Parse(data_reader["id"].ToString()),
                    IsActive = Boolean.Parse(data_reader["isActive"].ToString()),
                    Projectname = data_reader["projectname"].ToString(),
                    Projectnumber = data_reader["projectnumber"].ToString(),
                    Productionplace = data_reader["productionplace"].ToString(),
                    Serialnumber = data_reader["serialnumber"].ToString(),
                    //Worker = data_reader["worker"].ToString(),
                };
            }

            data_reader.Close();

            return new Diagramm();
        }

        public void safeDiagramm(Diagramm diagramm)
        {
            //tblDiagramm tbl_diag = diagramm;
            
        }

        public void addDiagramm(Diagramm diagramm)
        {

            sql_cmd = new SqlCommand();
            sql_cmd.Connection = sql_connection;

            //Parameters for tblDiagramm
            sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.VarChar, 50).Value = diagramm.ID;
            sql_cmd.Parameters.Add("@Customer", SqlDbType.VarChar, 50).Value = diagramm.Customer;
            sql_cmd.Parameters.Add("@EndCustomer", SqlDbType.VarChar, 50).Value = diagramm.Endcustomer;
            sql_cmd.Parameters.Add("@FieldName", SqlDbType.VarChar, 50).Value = diagramm.Fieldname;
            sql_cmd.Parameters.Add("@JobNumber", SqlDbType.VarChar, 50).Value = diagramm.Jobumber;
            sql_cmd.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 50).Value = diagramm.Serialnumber;
            sql_cmd.Parameters.Add("@ProjectNumber", SqlDbType.VarChar, 50).Value = diagramm.Projectnumber;
            sql_cmd.Parameters.Add("@ProjectName", SqlDbType.VarChar, 50).Value = diagramm.Projectname;
            sql_cmd.Parameters.Add("@AddressRow1", SqlDbType.VarChar, 50).Value = diagramm.SiteRow1;
            sql_cmd.Parameters.Add("@AddressRow2", SqlDbType.VarChar, 50).Value = diagramm.SiteRow2;
            sql_cmd.Parameters.Add("@AddressRow3", SqlDbType.VarChar, 50).Value = diagramm.SiteRow3;
            sql_cmd.Parameters.Add("@Date_Init", SqlDbType.DateTime2).Value = diagramm.Date_init;
            sql_cmd.Parameters.Add("@Date_LastChange", SqlDbType.DateTime2).Value = diagramm.Date_lastchange;
            //sql_cmd.Parameters.Add("@ProductionPlace", SqlDbType.VarChar, 10).Value = diagramm.Productionplace;
            sql_cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 50).Value = diagramm.IsActive;
            sql_cmd.Parameters.Add("@SourceFolder", SqlDbType.VarChar, 50).Value = diagramm.SourceFolder;

            try //try to INSERT Diagrammdata, when not exists
            {
                sql_cmd.CommandText =   "IF NOT EXISTS (SELECT * FROM dbo.tblDiagramm WHERE Serialnumber = @SerialNumber AND FieldName = @FieldName AND D_id = @Diagramm_ID) " +
                                        "INSERT into dbo.tblDiagramm (D_id, Customer, EndCustomer, FieldName, JobNumber, SerialNumber, ProjectNumber, ProjectName, AddressRow1, AddressRow2, AddressRow3, Date_init, Date_lastChange, IsActive, SourceFolder) " +
                                        "VALUES (@Diagramm_ID, @Customer, @EndCustomer, @FieldName, @JobNumber, @SerialNumber, @ProjectNumber, @ProjectName, @AddressRow1, @AddressRow2, @AddressRow3, @Date_init, @Date_lastChange, @IsActive, @SourceFolder)";
                                        
                sql_cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }

            for (int i = 0; i < diagramm.pages_List.Count; i++ ) //Für jede Seite fügen einen Eintrag hinzu
            {
                sql_cmd = new SqlCommand();
                sql_cmd.Connection = sql_connection;

                sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.VarChar, 50).Value = diagramm.ID;
                sql_cmd.Parameters.Add("@Date_Init", SqlDbType.DateTime2).Value = diagramm.Date_init;
                sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;
                sql_cmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Title;
                sql_cmd.Parameters.Add("@PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].PrePreFix;
                sql_cmd.Parameters.Add("@PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].PreFix;
                sql_cmd.Parameters.Add("@OriginNumber", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].OriginNumber;
                sql_cmd.Parameters.Add("@Author", SqlDbType.VarChar, 10).Value = diagramm.pages_List[i].Author;

                try //Schreibe seitendaten , wenn nicht vorhanden,  in DB
                {
                    sql_cmd.CommandText =   "INSERT into dbo.tblPage (D_id, P_id, Title, PrePreFix, PreFix, OriginNumber, Author, Date_init) " +
                                            "VALUES (@Diagramm_ID, @Page_ID, @Title, @PrePreFix, @PreFix, @OriginNumber, @Author, @Date_Init)";
                    sql_cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }

                //Für jedes Bauteil dieser Seite, füge in Bauteil in DB ein, wenn nicht vorhanden
                for (int ii = 0; ii < diagramm.pages_List[i].Parts_List.Count; ii++)
                {
                    sql_cmd = new SqlCommand();
                    sql_cmd.Connection = sql_connection;

                    //sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.Int).Value = nextID.ToString();
                    //sql_cmd.Parameters.Add("@Date_Init", SqlDbType.DateTime2).Value = diagramm.Date_init;
                    sql_cmd.Parameters.Add("@Part_BMK", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].BMK;
                    sql_cmd.Parameters.Add("@Part_PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].PrePreFix;
                    sql_cmd.Parameters.Add("@Part_PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].PreFix;
                    sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;

                    try //Schreibe Part-Daten in DB, wenn nicht vorhanden
                    {
                        sql_cmd.CommandText =   "IF NOT EXISTS (SELECT * FROM dbo.tblPart WHERE P_id = @Page_ID AND BMK = @Part_BMK) " +
                                                "INSERT into dbo.tblPart (P_id, BMK, PrePreFix, PreFix) " +
                                                "VALUES (@Page_ID, @Part_BMK, @Part_PrePreFix, @Part_PreFix)";
                        sql_cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }

                //Für jedes Potential dieser Seite, füge in Bauteil in DB ein, wenn nicht vorhanden
                for (int iii = 0; iii < diagramm.pages_List[i].Potential_List.Count; iii++)
                {
                    sql_cmd = new SqlCommand();
                    sql_cmd.Connection = sql_connection;

                    
                    //sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.Int).Value = nextID.ToString();
                    sql_cmd.Parameters.Add("@Potential_Name", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].Name;
                    sql_cmd.Parameters.Add("@Potential_PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].PrePreFix;
                    sql_cmd.Parameters.Add("@Potential_PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].PreFix;
                    sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;

                    try//Schreibe Potential-Daten in DB, wenn nicht vorhanden
                    {
                        sql_cmd.CommandText =   "IF NOT EXISTS (SELECT * FROM dbo.tblPotential WHERE P_id = @Page_ID AND Name = @Potential_Name) " +
                                                "INSERT into dbo.tblPotential (P_id, Name, PrePreFix, PreFix) " +
                                                "VALUES (@Page_ID, @Potential_Name, @Potential_PrePreFix, @Potential_PreFix)";
                        sql_cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
                
            }
        }
    }
}
