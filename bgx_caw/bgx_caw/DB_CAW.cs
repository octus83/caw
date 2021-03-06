﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;
using System.Windows;
using System.Configuration;
using System.IO;
using System.Windows.Media.Imaging;

namespace bgx_caw
{
    /// <summary>
    /// partial class that implements main functions to connect to fixed database
    /// </summary>
    partial class DB_CAW : IDisposable
    {
        private Configuration config;

        private SqlConnection sql_connection;
        private SqlConnectionStringBuilder connection_string = new SqlConnectionStringBuilder();

        private Logger logger;

        /// <summary>
        /// calls SqlConnection.Open to open connection to database
        /// </summary>
        public DB_CAW()
        {
            config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            logger = new Logger();
            connection_string.DataSource = config.AppSettings.Settings["DataSource"].Value;
            connection_string.InitialCatalog = config.AppSettings.Settings["InitialCatalog"].Value;
            connection_string.IntegratedSecurity = true;

            sql_connection = new SqlConnection(connection_string.ConnectionString);
            sql_connection.Open();
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

        public List<Diagramm> getDiagramms() //Liste mit allen Diagrammen, allerdings nur "Grunddaten" ohne Seiten/Potentiale/Bauteile etc
        {
            sql_cmd = new SqlCommand("SELECT * FROM dbo.tbldiagramm");
            sql_cmd.Connection = sql_connection;
            SqlDataReader data_reader = sql_cmd.ExecuteReader();
            List<Diagramm> result_list = new List<Diagramm>();

            while (data_reader.Read())
            {
                result_list.Add(new Diagramm
                {
                    ID = data_reader["D_id"].ToString(),
                    SerialNumber = data_reader["serialnumber"].ToString(),
                    FieldName = data_reader["fieldname"].ToString(),
                    JobNumber = data_reader["JobNumber"].ToString(),
                    ProjectNumber = data_reader["projectnumber"].ToString(),
                    ProjectName = data_reader["projectname"].ToString(),
                    Date_Init = DateTime.Parse(data_reader["date_init"].ToString()),
                    Date_LastChange = DateTime.Parse(data_reader["date_lastchange"].ToString()),
                    IsActive = Boolean.Parse(data_reader["isActive"].ToString())
                });
            }

            data_reader.Close();

            result_list.Sort((x, y) => DateTime.Compare(x.Date_LastChange, y.Date_LastChange));

            result_list.Reverse();

            return result_list;

        }

        public Diagramm getDiagramm(String id) //Ein komplettes Diagramm-Objekt
        {
            sql_cmd = new SqlCommand("SELECT * FROM dbo.tbldiagramm WHERE D_ID ='" + id + "'");
            sql_cmd.Connection = sql_connection;
            SqlDataReader data_reader = sql_cmd.ExecuteReader();
            Diagramm result = new Diagramm();

            while (data_reader.Read())
            {
                result = new Diagramm
                {
                    ID = data_reader["D_id"].ToString(),
                    Customer = data_reader["Customer"].ToString(),
                    EndCustomer = data_reader["EndCustomer"].ToString(),
                    FieldName = data_reader["FieldName"].ToString(),
                    JobNumber = data_reader["JobNumber"].ToString(),
                    SerialNumber = data_reader["SerialNumber"].ToString(),
                    ProjectNumber = data_reader["ProjectNumber"].ToString(),
                    ProjectName = data_reader["ProjectName"].ToString(),
                    AddressRow1 = data_reader["AddressRow1"].ToString(),
                    AddressRow2 = data_reader["AddressRow2"].ToString(),
                    AddressRow3 = data_reader["AddressRow3"].ToString(),
                    Date_Init = DateTime.Parse(data_reader["Date_Init"].ToString()),
                    Date_LastChange = DateTime.Parse(data_reader["Date_Lastchange"].ToString()),
                    ProductionPlace = data_reader["ProductionPlace"].ToString(),
                    IsActive = Boolean.Parse(data_reader["isActive"].ToString())               
                };
            }

            data_reader.Close();


            sql_cmd = new SqlCommand("SELECT * FROM dbo.tblPage WHERE D_id = '" + id + "'");
            sql_cmd.Connection = sql_connection;

            data_reader = sql_cmd.ExecuteReader();

            while (data_reader.Read())
            {
                result.pages_List.Add(new Page
                {
                    D_id = result.ID,
                    Date_Init = DateTime.Parse(data_reader["Date_Init"].ToString()),
                    Date_LastChange = DateTime.Parse(data_reader["Date_Lastchange"].ToString()),
                    OriginNumber = data_reader["OriginNumber"].ToString(),
                    P_id = data_reader["P_id"].ToString(),
                    PreFix = data_reader["PreFix"].ToString(),
                    PrePreFix = data_reader["PrePreFix"].ToString(),
                    Title = data_reader["Title"].ToString(),
                    PageInDiagramm = Int32.Parse(data_reader["PageInDiagramm"].ToString()),
                    /*Version = Int32.Parse(data_reader["Version"].ToString())*/
                });
            }

            data_reader.Close();

            foreach (var page in result.pages_List)
            {
                sql_cmd = new SqlCommand("SELECT * FROM dbo.tblPotential WHERE P_id = '" + page.P_id + "'");
                sql_cmd.Connection = sql_connection;

                data_reader = sql_cmd.ExecuteReader();

                result.pages_List[result.pages_List.IndexOf(page)].Potential_List = new List<Potential>();

                while (data_reader.Read())
                {
                    result.pages_List[result.pages_List.IndexOf(page)].Potential_List.Add(new Potential
                    {
                        D_id = result.ID,
                        Name = data_reader["Name"].ToString(),
                        P_id = data_reader["P_id"].ToString(),
                        PreFix = data_reader["PreFix"].ToString(),
                        PrePreFix = data_reader["PrePreFix"].ToString(),

                    });
                }

                data_reader.Close();

                sql_cmd = new SqlCommand("SELECT * FROM dbo.tblPart WHERE P_id = '" + page.P_id + "'");
                sql_cmd.Connection = sql_connection;

                data_reader = sql_cmd.ExecuteReader();

                result.pages_List[result.pages_List.IndexOf(page)].Parts_List = new List<Part>();

                while (data_reader.Read())
                {
                    result.pages_List[result.pages_List.IndexOf(page)].Parts_List.Add(new Part
                    {
                        D_id = result.ID,
                        BMK = data_reader["BMK"].ToString(),
                        P_id = data_reader["P_id"].ToString(),
                        PreFix = data_reader["PreFix"].ToString(),
                        PrePreFix = data_reader["PrePreFix"].ToString(),

                    });
                }

                data_reader.Close();
            }

            return result;
        }

        public void addDiagramm(Diagramm diagramm) //Fügt Diagramm-Objekt in Datenbank ein
        {
            sql_cmd = new SqlCommand();
            sql_cmd.Connection = sql_connection;

            //Parameters for tblDiagramm
            sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.VarChar, 50).Value = diagramm.ID;
            sql_cmd.Parameters.Add("@Customer", SqlDbType.VarChar, 50).Value = diagramm.Customer;
            sql_cmd.Parameters.Add("@EndCustomer", SqlDbType.VarChar, 50).Value = diagramm.EndCustomer;
            sql_cmd.Parameters.Add("@FieldName", SqlDbType.VarChar, 50).Value = diagramm.FieldName;
            sql_cmd.Parameters.Add("@JobNumber", SqlDbType.VarChar, 50).Value = diagramm.JobNumber;
            sql_cmd.Parameters.Add("@SerialNumber", SqlDbType.VarChar, 50).Value = diagramm.SerialNumber;
            sql_cmd.Parameters.Add("@ProjectNumber", SqlDbType.VarChar, 50).Value = diagramm.ProjectNumber;
            sql_cmd.Parameters.Add("@ProjectName", SqlDbType.VarChar, 50).Value = diagramm.ProjectName;
            sql_cmd.Parameters.Add("@AddressRow1", SqlDbType.VarChar, 50).Value = diagramm.AddressRow1;
            sql_cmd.Parameters.Add("@AddressRow2", SqlDbType.VarChar, 50).Value = diagramm.AddressRow2;
            sql_cmd.Parameters.Add("@AddressRow3", SqlDbType.VarChar, 50).Value = diagramm.AddressRow3;
            sql_cmd.Parameters.Add("@Date_Init", SqlDbType.DateTime2).Value = diagramm.Date_Init;
            sql_cmd.Parameters.Add("@Date_LastChange", SqlDbType.DateTime2).Value = diagramm.Date_LastChange;
            sql_cmd.Parameters.Add("@IsActive", SqlDbType.Bit, 50).Value = diagramm.IsActive;
            sql_cmd.Parameters.Add("@SourceFolder", SqlDbType.VarChar, 50).Value = System.IO.Path.Combine(config.AppSettings.Settings["ProgrammPath"].Value, diagramm.ID);

            try //try to INSERT Diagrammdata, when not exists
            {
                sql_cmd.CommandText = "IF NOT EXISTS (SELECT * FROM dbo.tblDiagramm WHERE Serialnumber = @SerialNumber AND FieldName = @FieldName AND D_id = @Diagramm_ID) " +
                                        "INSERT into dbo.tblDiagramm (D_id, Customer, EndCustomer, FieldName, JobNumber, SerialNumber, ProjectNumber, ProjectName, AddressRow1, AddressRow2, AddressRow3, Date_init, Date_lastChange, IsActive, SourceFolder) " +
                                        "VALUES (@Diagramm_ID, @Customer, @EndCustomer, @FieldName, @JobNumber, @SerialNumber, @ProjectNumber, @ProjectName, @AddressRow1, @AddressRow2, @AddressRow3, @Date_init, @Date_lastChange, @IsActive, @SourceFolder)";

                sql_cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                //throw;
                MessageBox.Show(exc.Message);
            }

            for (int i = 0; i < diagramm.pages_List.Count; i++) //Für jede Seite fügen einen Eintrag hinzu
            {
                sql_cmd = new SqlCommand();
                sql_cmd.Connection = sql_connection;

                sql_cmd.Parameters.Add("@Diagramm_ID", SqlDbType.VarChar, 50).Value = diagramm.ID;
                sql_cmd.Parameters.Add("@Date_Init", SqlDbType.DateTime2).Value = diagramm.Date_Init;
                sql_cmd.Parameters.Add("@Date_LastChange", SqlDbType.DateTime2).Value = diagramm.Date_LastChange;
                sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;
                sql_cmd.Parameters.Add("@Title", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Title;
                sql_cmd.Parameters.Add("@PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].PrePreFix;
                sql_cmd.Parameters.Add("@PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].PreFix;
                sql_cmd.Parameters.Add("@OriginNumber", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].OriginNumber;
                sql_cmd.Parameters.Add("@Author", SqlDbType.VarChar, 10).Value = diagramm.pages_List[i].Author;
                sql_cmd.Parameters.Add("@PageInDiagramm", SqlDbType.Int).Value = diagramm.pages_List[i].PageInDiagramm;
                sql_cmd.Parameters.Add("@Image", SqlDbType.Image, diagramm.pages_List[i].Image.Length).Value = diagramm.pages_List[i].Image;

                try //Schreibe seitendaten , wenn nicht vorhanden,  in DB
                {
                    sql_cmd.CommandText = "INSERT into dbo.tblPage (D_id, P_id, Title, PrePreFix, PreFix, OriginNumber, Author, Date_init, Date_LastChange, PageInDiagramm, BLOB) " +
                                            "VALUES (@Diagramm_ID, @Page_ID, @Title, @PrePreFix, @PreFix, @OriginNumber, @Author, @Date_Init, @Date_LastChange, @PageInDiagramm, @Image)";
                    sql_cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    //throw;
                    MessageBox.Show(exc.Message);
                }

                //Für jedes Bauteil dieser Seite, füge in Bauteil in DB ein, wenn nicht vorhanden
                for (int ii = 0; ii < diagramm.pages_List[i].Parts_List.Count; ii++)
                {
                    sql_cmd = new SqlCommand();
                    sql_cmd.Connection = sql_connection;

                    sql_cmd.Parameters.Add("@Part_BMK", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].BMK;
                    sql_cmd.Parameters.Add("@Part_PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].PrePreFix;
                    sql_cmd.Parameters.Add("@Part_PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Parts_List[ii].PreFix;
                    sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;

                    try //Schreibe Part-Daten in DB, wenn nicht vorhanden
                    {
                        sql_cmd.CommandText = "IF NOT EXISTS (SELECT * FROM dbo.tblPart WHERE P_id = @Page_ID AND BMK = @Part_BMK) " +
                                                "INSERT into dbo.tblPart (P_id, BMK, PrePreFix, PreFix) " +
                                                "VALUES (@Page_ID, @Part_BMK, @Part_PrePreFix, @Part_PreFix)";
                        sql_cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        //throw;
                        //Delete all Data to this Diagramm
                        MessageBox.Show(exc.Message);
                    }
                }

                //Für jedes Potential dieser Seite, füge in Bauteil in DB ein, wenn nicht vorhanden
                for (int iii = 0; iii < diagramm.pages_List[i].Potential_List.Count; iii++)
                {
                    sql_cmd = new SqlCommand();
                    sql_cmd.Connection = sql_connection;

                    sql_cmd.Parameters.Add("@Potential_Name", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].Name;
                    sql_cmd.Parameters.Add("@Potential_PrePreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].PrePreFix;
                    sql_cmd.Parameters.Add("@Potential_PreFix", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].Potential_List[iii].PreFix;
                    sql_cmd.Parameters.Add("@Page_ID", SqlDbType.VarChar, 50).Value = diagramm.pages_List[i].P_id;

                    try//Schreibe Potential-Daten in DB, wenn nicht vorhanden
                    {
                        sql_cmd.CommandText = "IF NOT EXISTS (SELECT * FROM dbo.tblPotential WHERE P_id = @Page_ID AND Name = @Potential_Name) " +
                                                "INSERT into dbo.tblPotential (P_id, Name, PrePreFix, PreFix) " +
                                                "VALUES (@Page_ID, @Potential_Name, @Potential_PrePreFix, @Potential_PreFix)";
                        sql_cmd.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        //throw;
                        //Delete all Data to this Diagramm
                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }

        public void deleteDiagramm(String id) //Löscht alle Einträge zu einer Diagramm ID
        {
            List<String> pages = new List<String>();

            sql_cmd = new SqlCommand("SELECT P_id FROM dbo.tblPage WHERE D_id = '" + id + "'");
            sql_cmd.Connection = sql_connection;
            SqlDataReader data_reader = sql_cmd.ExecuteReader();
            Diagramm result = new Diagramm();

            while (data_reader.Read())
            {
                pages.Add(data_reader["P_id"].ToString());
            }

            data_reader.Close();

            foreach (var page in pages)
            {
                sql_cmd = new SqlCommand("DELETE FROM dbo.tblPart WHERE P_id = '" + page + "'");
                sql_cmd.Connection = sql_connection;
                sql_cmd.ExecuteNonQuery();

                sql_cmd = new SqlCommand("DELETE FROM dbo.tblPotential WHERE P_id = '" + page + "'");
                sql_cmd.Connection = sql_connection;
                sql_cmd.ExecuteNonQuery();
            }

            sql_cmd = new SqlCommand("DELETE FROM dbo.tblPage WHERE D_id = '" + id + "'");
            sql_cmd.Connection = sql_connection;
            sql_cmd.ExecuteNonQuery();

            sql_cmd = new SqlCommand("DELETE FROM dbo.tblDiagramm WHERE D_id = '" + id + "'");
            sql_cmd.Connection = sql_connection;
            sql_cmd.ExecuteNonQuery();

        }
        /// <summary>
        /// Liest die Bilder (Orginal und Custom BLOBS) und entsprechende Informationen dazu aus der
        /// Datenbank und speichert sie als CustomBitmapImage
        /// </summary>
        /// <param name="id">Seiten ID</param>
        /// <returns></returns>
        public CustomBitmapImage getBLOB(String id)
        {

            sql_cmd = new SqlCommand(
               "Select dbo.tblPage.P_id, dbo.tblPage.BLOB, dbo.tblPage.PageInDiagramm, dbo.tblPage.CustomBLOB FROM dbo.tblPage Where dbo.tblPage.P_id = '" + id + "'");
            sql_cmd.Connection = sql_connection;
            SqlDataReader data_reader = sql_cmd.ExecuteReader();
            CustomBitmapImage cbi = new CustomBitmapImage();
            while (data_reader.Read())
            {

                cbi.OrginalImage = createbitmapImage((byte[])data_reader["BLOB"]);
                //Falls kein custom Bild exestiert
                if (!data_reader.IsDBNull(3))
                {
                    cbi.CustomImage = createbitmapImage((byte[])data_reader["CustomBLOB"]);
                }
                else
                {
                    cbi.CustomImage = null;
                }                        
                cbi.P_id = data_reader["P_id"].ToString();
                cbi.PageInDiagramm = ((int)(data_reader["PageInDiagramm"])+1);
            }
            data_reader.Close();
            return cbi;
        }
        /// <summary>
        /// Liest die Bilder (Orginal und Custom BLOBS) und entsprechende Informationen dazu aus der
        /// Datenbank und speichert sie als CustomBitmapImage mit Priorität als eigenständige Verbindung
        /// </summary>
        /// <param name="id">Seiten ID</param>
        /// <returns></returns>
        public CustomBitmapImage getBLOBFast(String id)
        {
            SqlConnection single_sql_connection;
            single_sql_connection = new SqlConnection(connection_string.ConnectionString);
            single_sql_connection.Open();
            SqlCommand single_sql_cmd;
            single_sql_cmd = new SqlCommand(
                 "Select dbo.tblPage.P_id, dbo.tblPage.BLOB, dbo.tblPage.PageInDiagramm, dbo.tblPage.CustomBLOB FROM dbo.tblPage Where dbo.tblPage.P_id = '" + id + "'");
            single_sql_cmd.Connection = single_sql_connection;
            SqlDataReader data_reader = single_sql_cmd.ExecuteReader();
            CustomBitmapImage cbi = new CustomBitmapImage();
            while (data_reader.Read())
            {
                cbi.OrginalImage = createbitmapImage((byte[])data_reader["BLOB"]);

                if (!data_reader.IsDBNull(3))
                {
                    cbi.CustomImage = createbitmapImage((byte[])data_reader["CustomBLOB"]);
                }
                else
                {
                    cbi.CustomImage = null;
                }        
                
                cbi.P_id = data_reader["P_id"].ToString();
                cbi.PageInDiagramm = ((int)(data_reader["PageInDiagramm"]) + 1);
            }
            data_reader.Close();
            single_sql_connection.Close();
            return cbi;
        }
        /// <summary>
        /// Erzeugt aus einem ByteArray ein BitmapImage
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        private BitmapImage createbitmapImage(byte[] imageBytes)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = new MemoryStream(imageBytes);
            bitmapImage.EndInit();
            return bitmapImage;
        }
        /// <summary>
        /// Schreibt ein ByteArray in die Page Tabelle ins CustomBLOB feld 
        /// </summary>
        /// <param name="b"></param>
        /// <param name="p_id"></param>
        public void writeCustomBlobToDatabase(byte[] b, String p_id)
        {
            sql_cmd = new SqlCommand();
            sql_cmd.Connection = sql_connection;
        
            try
            {
                sql_cmd.Parameters.Add("@CustomBLOB", SqlDbType.Image).Value = b;
                sql_cmd.CommandText = "UPDATE dbo.tblPage SET CustomBLOB= @CustomBLOB WHERE P_Id='" + p_id + "'";
                sql_cmd.ExecuteNonQuery();
                logger.log("successfully write custom blob to db", "DB_CAW.cs -> writeCustomBlobToDatabase");             
            }
            catch (Exception exc)
            {
                logger.log("Customblob write to DB fehlgeschlagen", "DB_CAW.cs -> writeCustomBlobToDatabase");
                logger.log(exc.Message, "DB_CAW.cs -> writeCustomBlobToDatabase");
            }
        }
    }
}