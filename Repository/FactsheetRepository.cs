using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using MySql.Data.MySqlClient;
using GBNNSS.Models;
using System.Xml;
using System.Text;

namespace GBNNSS.Repository
{
    public class FactsheetRepository
    {
        public string connStr = ConfigurationManager.ConnectionStrings["strGBnn"].ConnectionString;

        //public string cmdStr = "SELECT s.species_id, s.brc_concept, s.nbn_tvk, sn.name AS species_name, sn.authority, cn.name AS common_name, ht.phylum, " +
        //                        "ht.family, ht.orderr, eg.valuee AS environment, fg.valuee AS functional, st.valuee AS gbstatus, ste.valuee AS engstatus, " +
        //                        "sts.valuee AS scstatus, stw.valuee AS wastatus, ps.valuee AS gbps, pse.valuee AS engps, pss.valuee AS scps, psw.valuee AS waps, " +
        //                        "ih.place_first_found AS first_location, " +
        //                        "CASE ih.dt_id WHEN 1 THEN DAY(ih.startdate) " +
        //                        "WHEN 2 THEN CONCAT(DAY(ih.startdate), ' ',DAY(ih.enddate)) " +
        //                        "WHEN 3 THEN MONTH(ih.startdate) " +
        //                        "WHEN 4 THEN CONCAT(MONTH(ih.startdate), ' ', MONTH(ih.enddate)) " +
        //                        "WHEN 5 THEN YEAR(ih.startdate) " +
        //                        "WHEN 6 THEN CONCAT(YEAR(ih.startdate), ' ', YEAR(ih.enddate)) " +
        //                        "WHEN 7 THEN YEAR(ih.enddate) " +
        //                        "ELSE 'Unknown' " +
        //                        "END AS first_Date, " +
        //                        "pf.factsheet_id, pf.short_description, pf.biology_ecology, pf.habitat, pf.habitat_summary, pf.distribution, pf.impact, pf.impact_summary,pf.management," +
        //                        "pf.status_summary, pf.invasion_history, pf.legislation, pf.cited_references, pf.other_references, pf.authors, pf.last_updated " +
        //                        "FROM species s " +
        //                        "LEFT JOIN species_name sn ON sn.species_id = s.species_id AND sn.nt_id = 1 AND sn.valid = 1 " +
        //                        "LEFT JOIN species_name cn ON cn.species_id = s.species_id AND cn.nt_id = 2 AND cn.valid = 1 " +
        //                        "LEFT JOIN higher_taxonomy ht ON ht.ht_id = s.ht_id " +
        //                        "LEFT JOIN environmental_group eg ON eg.eg_id = s.eg_id " +
        //                        "LEFT JOIN functional_group fg ON fg.fg_id = s.fg_id " +
        //                        "LEFT JOIN sir_ps_id ss ON ss.species_id = s.species_id AND ss.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Great Britain') " +
        //                        "LEFT JOIN sir_ps_id sse ON sse.species_id = s.species_id AND sse.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'England') " +
        //                        "LEFT JOIN sir_ps_id sss ON sss.species_id = s.species_id AND sss.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Scotland') " +
        //                        "LEFT JOIN sir_ps_id ssw ON ssw.species_id = s.species_id AND ssw.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Wales') " +
        //                        "LEFT JOIN abundance ab ON ab.abundance_id = ss.abundance_id " +
        //                        "LEFT JOIN `status` st ON st.status_id  = ss.status_id " +
        //                        "LEFT JOIN `status` ste ON ste.status_id = sse.status_id " +
        //                        "LEFT JOIN `status` sts ON sts.status_id = sss.status_id " +
        //                        "LEFT JOIN `status` stw ON stw.status_id = ssw.status_id " +
        //                        "LEFT JOIN population_status ps ON ps.ps_id = ss.population_status " +
        //                        "LEFT JOIN population_status pse ON pse.ps_id = sse.population_status " +
        //                        "LEFT JOIN population_status pss ON pss.ps_id = sss.population_status " +
        //                        "LEFT JOIN population_status psw ON psw.ps_id = ssw.population_status " +
        //                        "LEFT JOIN invasion_history ih ON ih.species_id = s.species_id " +
        //                        "LEFT JOIN published_factsheets pf ON pf.factsheet_id = s.factsheet_id " +
        //                        "WHERE s.species_id = @species_id " +
        //                        "AND (ss.status_id NOT IN (9,10) OR sse.status_id NOT IN (9,10) OR sss.status_id NOT IN (9,10) OR ssw.status_id NOT IN (9,10)) ";

        public string cmdStr = "SELECT s.species_id, s.brc_concept, s.nbn_tvk, sn.name AS species_name, sn.authority, cn.name AS common_name, ht.phylum, " +
                                "ht.family, ht.orderr, eg.valuee AS environment, fg.valuee AS functional, st.valuee AS gbstatus, ste.valuee AS engstatus, " +
                                "sts.valuee AS scstatus, stw.valuee AS wastatus, ps.valuee AS gbps, pse.valuee AS engps, pss.valuee AS scps, psw.valuee AS waps, " +
                                "ih.place_first_found AS first_location, " +
                                "CASE ih.dt_id WHEN 1 THEN DAY(ih.startdate) " +
                                "WHEN 2 THEN CONCAT(DAY(ih.startdate), ' ',DAY(ih.enddate)) " +
                                "WHEN 3 THEN MONTH(ih.startdate) " +
                                "WHEN 4 THEN CONCAT(MONTH(ih.startdate), ' ', MONTH(ih.enddate)) " +
                                "WHEN 5 THEN YEAR(ih.startdate) " +
                                "WHEN 6 THEN CONCAT(YEAR(ih.startdate), ' ', YEAR(ih.enddate)) " +
                                "WHEN 7 THEN YEAR(ih.enddate) " +
                                "ELSE 'Unknown' " +
                                "END AS first_Date,  " +
                                "f.factsheet_id, f.short_description, f.biology_ecology, f.habitat, f.habitat_summary, f.distribution, f.impact, f.impact_summary,f.management," +
                                "f.status_summary, f.invasion_history, f.legislation, f.cited_references, f.other_references, f.authors, f.last_updated, " +
                                "CASE s.nativemapdisplay WHEN 1 THEN 'yes' ELSE 'no' END AS isnativemap, " +
                                "CASE s.distmapdisplay WHEN 1 THEN 'yes' ELSE 'no' END AS isdistmap " +
                                "FROM species s " +
                                "LEFT JOIN species_name sn ON sn.species_id = s.species_id AND sn.nt_id = 1 AND sn.valid = 1 " +
                                "LEFT JOIN species_name cn ON cn.species_id = s.species_id AND cn.nt_id = 2 AND cn.valid = 1 " +
                                "LEFT JOIN higher_taxonomy ht ON ht.ht_id = s.ht_id " +
                                "LEFT JOIN environmental_group eg ON eg.eg_id = s.eg_id " +
                                "LEFT JOIN functional_group fg ON fg.fg_id = s.fg_id " +
                                "LEFT JOIN species_in_region ss ON ss.species_id = s.species_id AND ss.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Great Britain') " +
                                "LEFT JOIN species_in_region sse ON sse.species_id = s.species_id AND sse.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'England') " +
                                "LEFT JOIN species_in_region sss ON sss.species_id = s.species_id AND sss.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Scotland') " +
                                "LEFT JOIN species_in_region ssw ON ssw.species_id = s.species_id AND ssw.region_id = (SELECT r.region_id FROM region r WHERE r.`name` = 'Wales') " +
                                "LEFT JOIN abundance ab ON ab.abundance_id = ss.abundance_id " +
                                "LEFT JOIN `status` st ON st.status_id  = ss.status_id " +
                                "LEFT JOIN `status` ste ON ste.status_id = sse.status_id " +
                                "LEFT JOIN `status` sts ON sts.status_id = sss.status_id " +
                                "LEFT JOIN `status` stw ON stw.status_id = ssw.status_id " +
                                "LEFT JOIN population_status ps ON ps.ps_id = ss.ps_id " +
                                "LEFT JOIN population_status pse ON pse.ps_id = sse.ps_id " +
                                "LEFT JOIN population_status pss ON pss.ps_id = sss.ps_id " +
                                "LEFT JOIN population_status psw ON psw.ps_id = ssw.ps_id " +
                                "LEFT JOIN invasion_history ih ON ih.species_id = s.species_id " +
                                "LEFT JOIN factsheets f ON f.factsheet_id = s.factsheet_id AND f.verified = 1 " +
                                "WHERE s.species_id = @species_id AND s.verified = 1 " +
                                "AND (ss.status_id NOT IN (9,10) OR sse.status_id NOT IN (9,10) OR sss.status_id NOT IN (9,10) OR ssw.status_id NOT IN (9,10)) ";

        MySqlConnection myConn = null;
        MySqlDataReader myReader = null;

        List<Species> species = new List<Species>();

        public FactsheetRepository()
        {
            var requestUrl = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Url.Segments.Last());
            /*Setup for array of Segments' id number is for Live server, Change this for local machine if require. */
            int idspecies = 0;
            int n;
            bool id = int.TryParse(requestUrl, out n);
            if (requestUrl.Length > 0 && id == true)
            {
                idspecies = int.Parse(requestUrl);
                try
                {
                    myConn = new MySqlConnection(connStr);
                    myConn.Open();
                    MySqlCommand myCmd = new MySqlCommand(cmdStr, myConn);
                    myCmd.Prepare();

                    myCmd.Parameters.Add("@species_id", MySqlDbType.Int32).Value = idspecies;
                    myReader = myCmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        PopulationStatus[] ps = new PopulationStatus[]
                    {
                        new PopulationStatus{
                        speciesid = myReader.IsDBNull(0) ? 0 : myReader.GetInt32(0),
                        gb = myReader.IsDBNull(15) ? "" : myReader.GetString(15),
                        england = myReader.IsDBNull(16) ? "" : myReader.GetString(16),
                        scotland = myReader.IsDBNull(17) ? "" : myReader.GetString(17),
                        wales = myReader.IsDBNull(18) ? "" : myReader.GetString(18)
                            }
                    };
                        SpeciesStatus[] ss = new SpeciesStatus[]
                    {
                        new SpeciesStatus{
                        speciesid = myReader.IsDBNull(0) ? 0 : myReader.GetInt32(0),
                        gb = myReader.IsDBNull(11) ? "" : myReader.GetString(11),
                        england = myReader.IsDBNull(12) ? "" : myReader.GetString(12),
                        scotland = myReader.IsDBNull(13) ? "" : myReader.GetString(13),
                        wales = myReader.IsDBNull(14) ? "" : myReader.GetString(14)                                                
                        }
                    };
                        Factsheet[] fs = new Factsheet[]
                    {
                        new Factsheet{
                        fact_id = myReader.IsDBNull(21) ? 0 : myReader.GetInt32(21),
                        short_desc = myReader.IsDBNull(22) ? "" : WriteXml(myReader.GetString(22)),
                        biology_ecology = myReader.IsDBNull(23) ? "" : WriteXml(myReader.GetString(23)),
                        habitat = myReader.IsDBNull(24) ? "" : WriteXml(myReader.GetString(24)),
                        habitat_summary = myReader.IsDBNull(25) ? "" : WriteXml(myReader.GetString(25)),
                        distribution = myReader.IsDBNull(26) ? "" : WriteXml(myReader.GetString(26)),
                        impact = myReader.IsDBNull(27) ? "" : WriteXml(myReader.GetString(27)),
                        impact_summary = myReader.IsDBNull(28) ? "" : WriteXml(myReader.GetString(28)),
                        management = myReader.IsDBNull(29) ? "" : WriteXml(myReader.GetString(29)),
                        status_summary = myReader.IsDBNull(30) ? "" : WriteXml(myReader.GetString(30)),
                        invasion_history = myReader.IsDBNull(31) ? "" : WriteXml(myReader.GetString(31)),
                        references = myReader.IsDBNull(33) ? "" : WriteXml(myReader.GetString(33)),
                        other_references = myReader.IsDBNull(34) ? "" : WriteXml(myReader.GetString(34)),
                        authors = myReader.IsDBNull(35) ? "" : WriteXml(myReader.GetString(35)),
                        last_updated = myReader.IsDBNull(36) ? "" : WriteXml(myReader.GetString(36))                            }
                    };
                        //legislation = myReader.IsDBNull(32) ? "" : WriteXml(myReader.GetString(32)),

                        List<string> syn = getSynonyms(idspecies);
                        List<string> cmn = getCommonNames(idspecies);
                        List<string> nrange = getNativeRanges(idspecies);

                        Species[] sp = new Species[]
                    {
                        new Species{
                            speciesid = myReader.IsDBNull(0) ? 0 : myReader.GetInt32(0),
                            brcnumber = myReader.IsDBNull(1) ? "" : myReader.GetString(1),
                            nbntvk = myReader.IsDBNull(2) ? "" : myReader.GetString(2),
                            latinname = myReader.IsDBNull(3) ? "" : myReader.GetString(3),
                            authority = myReader.IsDBNull(4) ? "" : myReader.GetString(4),
                            phylum = myReader.IsDBNull(6) ? "" : myReader.GetString(6),
                            family = myReader.IsDBNull(7) ? "" : myReader.GetString(7),
                            order = myReader.IsDBNull(8) ? "" : myReader.GetString(8),
                            environment = myReader.IsDBNull(9) ? "" : myReader.GetString(9),
                            functional = myReader.IsDBNull(10) ? "" : myReader.GetString(10),
                            locationfound = myReader.IsDBNull(19) ? "" : myReader.GetString(19),
                            firstdate = myReader.IsDBNull(20) ? "" : myReader.GetString(20),
                            factsheet = fs.Count<Factsheet>() == 0 ? null : fs[0],
                            speciesStatus = ss.Count<SpeciesStatus>() == 0 ? null : ss[0],
                            populationStatus = ps.Count<PopulationStatus>() == 0 ? null : ps[0],
                            CommonNames = cmn,
                            Synonyms =  syn,
                            NativeRanges = nrange,
                            displaynativemap = myReader.IsDBNull(37) ? "no" : myReader.GetString(37),
                            displaydistributionmap = myReader.IsDBNull(38) ? "no" : myReader.GetString(38),
                        }
                    };
                        species.Add(sp[0]);
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
            if (species == null)
            {
                // option 1 (throw)
                var notFoundMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(notFoundMessage);
            }
        }

        public IEnumerable<Species> GetFactsheets()
        {
            return species;
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

        public List<string> getNativeRanges(int idsp)
        {
            List<string> natRanges = new List<string>();
            string cmdStrNat = "SELECT r.`name` AS region_name FROM native_range nr LEFT JOIN region r ON r.region_id = nr.region_id WHERE nr.species_id = @species_id";
            MySqlDataReader myRdrNat = null;
            NativeRanges nr = new NativeRanges(); 
            try
            {
                myConn = new MySqlConnection(connStr);
                myConn.Open();
                MySqlCommand myCmdCn = new MySqlCommand(cmdStrNat, myConn);
                myCmdCn.Prepare();
                myCmdCn.Parameters.Add("@species_id", MySqlDbType.Int32).Value = idsp;
                myRdrNat = myCmdCn.ExecuteReader();
                while (myRdrNat.Read())
                {
                    string range = myRdrNat.GetString(0);
                    natRanges.Add(range);
                }
            }
            catch (MySqlException mysqlErr)
            {
                Console.WriteLine("MySQL Error :" + mysqlErr.ToString());
            }
            finally
            {
                if (myRdrNat != null) myRdrNat.Close();
                if (myConn != null) myConn.Close();
            }
            return natRanges;
        }

        public string WriteXml(string writer)
        {

            XmlDocument xDoc = new XmlDocument();
            var xmlDt = xDoc.CreateCDataSection(writer);

            return xmlDt.OuterXml;

        }
    }
}