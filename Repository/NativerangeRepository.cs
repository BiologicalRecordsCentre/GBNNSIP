using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using GBNNSS.Models;

namespace GBNNSS.Repository
{
    public class NativerangeRepository
    {
        public string connStr = ConfigurationManager.ConnectionStrings["strGBnn"].ConnectionString;

        public string cmdStr = "SELECT r.object_id, nr.region_id FROM native_range nr " +
                               "LEFT JOIN region r ON r.region_id = nr.region_id " +
                               "WHERE nr.species_id = @species_id";

        MySqlConnection nrConn = null;
        MySqlDataReader nrReader = null;

        List<Natives> natives = new List<Natives>();
        public NativerangeRepository()
        {
            var ctx = HttpContext.Current;
            int idspecies = int.Parse(ctx.Request.Url.Segments.Last());
            try
            {
                nrConn = new MySqlConnection(connStr);
                nrConn.Open();
                MySqlCommand nrCmd = new MySqlCommand(cmdStr, nrConn);
                nrCmd.Prepare();
                nrCmd.Parameters.Add("@species_id", MySqlDbType.Int32).Value = idspecies;
                nrReader = nrCmd.ExecuteReader();
                while (nrReader.Read())
                {
                    if(nrReader.IsDBNull(0)){
                        natives.Add(new Natives{
                            objIds = 0,
                            regionIds = nrReader.GetInt32(1)
                        });

                    }else{
                        natives.Add(new Natives{
                            objIds = nrReader.GetInt32(0),
                            regionIds = 0
                        });
                    }
                }
            }
            catch (MySqlException mysqlErr)
            {
                Console.WriteLine("MySQL Error :" + mysqlErr.ToString());
            }
            finally
            {
                if (nrReader != null) nrReader.Close();
                if (nrConn != null) nrConn.Close();
            }
        }
        public IEnumerable<Natives> GetNativeRange()
        {
            return natives;
        }
    }
}