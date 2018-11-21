using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using GBNNSS.Models;
using System.Text.RegularExpressions;

namespace GBNNSS.Repository
{
    public class SpeciesRepository
    {
        private const string CacheKey = "SpeciesStore";
        public string connStr = ConfigurationManager.ConnectionStrings["strGBnn"].ConnectionString;
        //public string cmdStr = "SELECT sn.species_id, sn.name AS sciname " +
        //                       "FROM species_name sn " +
        //                       "WHERE LOWER(sn.name) LIKE @strName AND sn.nt_id=1 AND sn.valid=1 " +
        //                       "ORDER BY sn.species_id ";

        public string cmdStr = "SELECT DISTINCT sn.species_id,sn1.name as spname, sn2.name as common_name " +
                 "FROM species_name sn " +
                 "LEFT JOIN species_name sn1 ON sn1.species_id = sn.species_id AND sn1.nt_id=1 AND sn1.valid=1 " +
                 "LEFT JOIN species_name sn2 ON sn2.species_id = sn.species_id AND sn2.nt_id=2 AND sn2.valid=1 " +
                 "WHERE sn.name LIKE @strName OR sn.species_id = @idspecies " +
                 "ORDER By sn.species_id,sn.nt_id,sn.valid DESC";

        MySqlConnection myConn = null;
        MySqlDataReader myReader = null;

 
        List<SpeciesNames> speciesname = new List<SpeciesNames>();
        public SpeciesRepository()
        {
            string pattern = "^[A-Za-z]+$";
            string returnUrl = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Url.Segments.Last());
            Uri spUrl = HttpContext.Current.Request.Url;
            //string speciesnm = HttpUtility.ParseQueryString(spUrl.Query).Get("speciesnm");
            string speciesnm = HttpContext.Current.Request.QueryString.Get("speciesnm");
            //int speciesid = int.Parse(HttpUtility.ParseQueryString(spUrl.Query).Get("idspecies"));
            int speciesid = 0;
            if (HttpContext.Current.Request.QueryString.Get("idspecies") != null) speciesid = int.Parse(HttpContext.Current.Request.QueryString.Get("idspecies"));
            string strName = "";

            if (speciesnm != null)
            {
                strName = "%" + speciesnm + "%";
            }
            else
            {
                if (speciesid == 0 && !Regex.IsMatch(returnUrl, pattern))
                {
                    speciesid = int.Parse(returnUrl);
                }
                else
                {

                    if (speciesid == 0) { strName = "%" + returnUrl + "%"; }
                }
            }
            try
            {
                myConn = new MySqlConnection(connStr);
                myConn.Open();
                MySqlCommand myCmd = new MySqlCommand(cmdStr, myConn);
                myCmd.Prepare();
                myCmd.Parameters.Add("@strName", MySqlDbType.String).Value = strName;
                myCmd.Parameters.Add("@idspecies", MySqlDbType.Int16).Value = speciesid;
                myReader = myCmd.ExecuteReader();
                while (myReader.Read())
                {
                    speciesid = myReader.IsDBNull(0) ? 0 : myReader.GetInt32(0);
                    List<string> syn = getSynonyms(speciesid);
                    List<string> cmn = getCommonNames(speciesid);
                        speciesname.Add(new SpeciesNames{
                            speciesid = myReader.IsDBNull(0) ? 0 : myReader.GetInt32(0),
                            scientific = myReader.IsDBNull(0) ? "" : myReader.GetString(1),
                            latinnames2 = syn,
                            englishnames = cmn
                        });
                }

            }
            catch (MySqlException mysqlErr)
            {
                Console.WriteLine("MySQL Error :" + mysqlErr.ToString());
            }
            finally
            {
                if (myReader != null) myReader.Close();
                if (myConn != null) myConn.Close();
            }

        }
        public IEnumerable<SpeciesNames> GetSpecies()
        {
            return speciesname;
        }

        public List<string> getSynonyms(int idsp)
        {
            string cmdStrSyn = "SELECT name as synonym FROM species_name sn WHERE sn.species_id = @species_id AND sn.nt_id = 1 AND sn.valid = 0";
            //string synonyms = "";
            List<string> synonym = new List<string>();
            MySqlDataReader myRdrSyn = null;

            try
            {
                myConn = new MySqlConnection(connStr);
                myConn.Open();
                MySqlCommand myCmdSyn = new MySqlCommand(cmdStrSyn, myConn);
                myCmdSyn.Prepare();
                myCmdSyn.Parameters.Add("@species_id", MySqlDbType.Int32).Value = idsp;
                myRdrSyn = myCmdSyn.ExecuteReader();

                while (myRdrSyn.Read())
                {
                    if (!myRdrSyn.IsDBNull(0)) synonym.Add(myRdrSyn.GetString(0));
                }
            }
            catch (MySqlException mysqlErr)
            {
                Console.WriteLine("MySQL Error :" + mysqlErr.ToString());
            }
            finally
            {
                if (myRdrSyn != null) myRdrSyn.Close();
                if (myConn != null) myConn.Close();
            }

            return synonym;
        }

        public List<string> getCommonNames(int idsp)
        {
            List<string> cnName = new List<string>();
            string cmdStrCn = "SELECT name as cnname From species_name sn WHERE sn.species_id = @species_id AND sn.nt_id=2";
            MySqlDataReader myRdrCn = null;

            try
            {
                myConn = new MySqlConnection(connStr);
                myConn.Open();
                MySqlCommand myCmdCn = new MySqlCommand(cmdStrCn, myConn);
                myCmdCn.Prepare();
                myCmdCn.Parameters.Add("@species_id", MySqlDbType.Int32).Value = idsp;
                myRdrCn = myCmdCn.ExecuteReader();

                while (myRdrCn.Read())
                {
                    if (!myRdrCn.IsDBNull(0)) cnName.Add(myRdrCn.GetString(0));
                }
            }
            catch (MySqlException mysqlErr)
            {
                Console.WriteLine("MySQL Error :" + mysqlErr.ToString());
            }
            finally
            {
                if (myRdrCn != null) myRdrCn.Close();
                if (myConn != null) myConn.Close();
            }

            return cnName;
        }

    }
}