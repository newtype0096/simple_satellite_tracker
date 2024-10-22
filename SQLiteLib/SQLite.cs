using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using SQLiteLib.Records;

namespace SQLiteLib
{
    public class SQLite
    {
        private static bool _init;

        private string _connString;

        public SQLite(string databaseFileName)
        {
            if (!File.Exists(databaseFileName))
            {
                SQLiteConnection.CreateFile(databaseFileName);
            }

            _connString = $"Data Source={databaseFileName}";
        }

        public void Init()
        {
            if (_init)
            {
                return;
            }

            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                string sql = "CREATE TABLE IF NOT EXISTS last_update (category TEXT PRIMARY KEY, datetime TEXT)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                sql = "CREATE TABLE IF NOT EXISTS satcat (" +
                    "object_name TEXT, object_id TEXT, norad_cat_id TEXT PRIMARY KEY, object_type TEXT, " +
                    "ops_status_code TEXT, owner TEXT, launch_date TEXT, launch_site TEXT, decay_date TEXT, " +
                    "period TEXT, inclination TEXT, apogee TEXT, perigee TEXT, rcs TEXT, data_status_code TEXT, " +
                    "orbit_center TEXT, orbit_type TEXT)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                sql = "CREATE TABLE IF NOT EXISTS gp_data (" +
                    "object_name TEXT, object_id TEXT, epoch TEXT, mean_motion TEXT, eccentricity TEXT, inclination TEXT, " +
                    "ra_of_asc_node TEXT, arg_of_pericenter TEXT, mean_anomaly TEXT, ephemeris_type TEXT, classification_type TEXT, " +
                    "norad_cat_id TEXT PRIMARY KEY, element_set_no TEXT, rev_at_epoch TEXT, bstart TEXT, mean_motion_dot TEXT, " +
                    "mean_motion_ddot TEXT, line1 TEXT, line2 TEXT, update_datetime TEXT)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            _init = true;
        }

        public bool SelectLastUpdate(string category, out LastUpdate lastUpdate)
        {
            lastUpdate = null;

            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = $"SELECT category, datetime FROM last_update WHERE category = '{category}'";

                    var cmd = new SQLiteCommand(sql, conn);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            lastUpdate = new LastUpdate()
                            {
                                CATEGORY = rdr["category"].ToString(),
                                DATETIME = DateTime.Parse(rdr["datetime"].ToString())
                            };
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        public bool UpsertLastUpdate(LastUpdate lastUpdate)
        {
            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "INSERT INTO last_update (category, datetime) " +
                                 "VALUES (@category, @datetime) " +
                                 "ON CONFLICT(category) DO UPDATE SET datetime = @datetime;";

                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@category", lastUpdate.CATEGORY);
                    cmd.Parameters.AddWithValue("@datetime", lastUpdate.DATETIME.ToString("yyyy-MM-dd 00:00:00"));
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        public bool SelectSatCats(out List<SatCat> satCats)
        {
            satCats = new List<SatCat>();

            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "SELECT object_name, object_id, norad_cat_id, object_type, ops_status_code, owner, " +
                        "launch_date, launch_site, decay_date, period, inclination, apogee, perigee, rcs, data_status_code, " +
                        "orbit_center, orbit_type FROM satcat";

                    var cmd = new SQLiteCommand(sql, conn);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var satcat = new SatCat();
                            satcat.OBJECT_NAME = rdr["object_name"].ToString();
                            satcat.OBJECT_ID = rdr["object_id"].ToString();
                            satcat.NORAD_CAT_ID = rdr["norad_cat_id"].ToString();
                            satcat.OBJECT_TYPE = rdr["object_type"].ToString();
                            satcat.OPS_STATUS_CODE = rdr["ops_status_code"].ToString();
                            satcat.OWNER = rdr["owner"].ToString();
                            satcat.LAUNCH_DATE = rdr["launch_date"].ToString();
                            satcat.LAUNCH_SITE = rdr["launch_site"].ToString();
                            satcat.DECAY_DATE = rdr["decay_date"].ToString();
                            satcat.PERIOD = rdr["period"].ToString();
                            satcat.INCLINATION = rdr["inclination"].ToString();
                            satcat.APOGEE = rdr["apogee"].ToString();
                            satcat.PERIGEE = rdr["perigee"].ToString();
                            satcat.RCS = rdr["rcs"].ToString();
                            satcat.DATA_STATUS_CODE = rdr["data_status_code"].ToString();
                            satcat.ORBIT_CENTER = rdr["orbit_center"].ToString();
                            satcat.ORBIT_TYPE = rdr["orbit_type"].ToString();

                            satCats.Add(satcat);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        public bool UpsertSatCats(List<SatCat> satCats)
        {
            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    BeginTransaction(conn);

                    foreach (var satCat in satCats)
                    {
                        string sql = "INSERT INTO satcat (object_name, object_id, norad_cat_id, object_type, ops_status_code, owner, " +
                                                "launch_date, launch_site, decay_date, period, inclination, apogee, perigee, rcs, data_status_code, " +
                                                "orbit_center, orbit_type) " +
                                                "VALUES (@object_name, @object_id, @norad_cat_id, @object_type, @ops_status_code, @owner, " +
                                                "@launch_date, @launch_site, @decay_date, @period, @inclination, @apogee, @perigee, @rcs, " +
                                                "@data_status_code, @orbit_center, @orbit_type) " +
                                                "ON CONFLICT(norad_cat_id) DO UPDATE SET " +
                                                "object_name = @object_name, " +
                                                "object_id = @object_id, " +
                                                "object_type = @object_type, " +
                                                "ops_status_code = @ops_status_code, " +
                                                "owner = @owner, " +
                                                "launch_date = @launch_date, " +
                                                "launch_site = @launch_site, " +
                                                "decay_date = @decay_date, " +
                                                "period = @period, " +
                                                "inclination = @inclination, " +
                                                "apogee = @apogee, " +
                                                "perigee = @perigee, " +
                                                "rcs = @rcs, " +
                                                "data_status_code = @data_status_code, " +
                                                "orbit_center = @orbit_center, " +
                                                "orbit_type = @orbit_type;";

                        var cmd = new SQLiteCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@object_name", satCat.OBJECT_NAME);
                        cmd.Parameters.AddWithValue("@object_id", satCat.OBJECT_ID);
                        cmd.Parameters.AddWithValue("@norad_cat_id", satCat.NORAD_CAT_ID);
                        cmd.Parameters.AddWithValue("@object_type", satCat.OBJECT_TYPE);
                        cmd.Parameters.AddWithValue("@ops_status_code", satCat.OPS_STATUS_CODE);
                        cmd.Parameters.AddWithValue("@owner", satCat.OWNER);
                        cmd.Parameters.AddWithValue("@launch_date", satCat.LAUNCH_DATE);
                        cmd.Parameters.AddWithValue("@launch_site", satCat.LAUNCH_SITE);
                        cmd.Parameters.AddWithValue("@decay_date", satCat.DECAY_DATE);
                        cmd.Parameters.AddWithValue("@period", satCat.PERIOD);
                        cmd.Parameters.AddWithValue("@inclination", satCat.INCLINATION);
                        cmd.Parameters.AddWithValue("@apogee", satCat.APOGEE);
                        cmd.Parameters.AddWithValue("@perigee", satCat.PERIGEE);
                        cmd.Parameters.AddWithValue("@rcs", satCat.RCS);
                        cmd.Parameters.AddWithValue("@data_status_code", satCat.DATA_STATUS_CODE);
                        cmd.Parameters.AddWithValue("@orbit_center", satCat.ORBIT_CENTER);
                        cmd.Parameters.AddWithValue("@orbit_type", satCat.ORBIT_TYPE);
                        cmd.ExecuteNonQuery();
                    }

                    EndTransaction(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        public bool UpsertGpDatas(List<GpData> gpDatas)
        {
            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    BeginTransaction(conn);

                    foreach (var gpData in gpDatas)
                    {
                        string sql = "INSERT INTO gp_data (object_name, object_id, epoch, mean_motion, eccentricity, inclination, ra_of_asc_node, " +
                                     "arg_of_pericenter, mean_anomaly, ephemeris_type, classification_type, norad_cat_id, element_set_no, rev_at_epoch, " +
                                     "bstar, mean_motion_dot, mean_motion_ddot, line1, line2, update_datetime) " +
                                     "VALUES(@object_name, @object_id, @epoch, @mean_motion, @eccentricity, @inclination, @ra_of_asc_node, " +
                                     "@arg_of_pericenter, @mean_anomaly, @ephemeris_type, @classification_type, @norad_cat_id, @element_set_no, @rev_at_epoch, " +
                                     "@bstar, @mean_motion_dot, @mean_motion_ddot, @line1, @line2, @update_datetime) " +
                                     "ON CONFLICT(norad_cat_id) DO UPDATE SET " +
                                     "object_name = @object_name, " +
                                     "object_id = @object_id, " +
                                     "epoch = @epoch, " +
                                     "mean_motion = @mean_motion, " +
                                     "eccentricity = @eccentricity, " +
                                     "inclination = @inclination, " +
                                     "ra_of_asc_node = @ra_of_asc_node, " +
                                     "arg_of_pericenter = @arg_of_pericenter, " +
                                     "mean_anomaly = @mean_anomaly, " +
                                     "ephemeris_type = @ephemeris_type, " +
                                     "classification_type = @classification_type, " +
                                     "element_set_no = @element_set_no, " +
                                     "rev_at_epoch = @rev_at_epoch, " +
                                     "bstar = @bstar, " +
                                     "mean_motion_dot = @mean_motion_dot, " +
                                     "mean_motion_ddot = @mean_motion_ddot, " +
                                     "line1 = @line1, " +
                                     "line2 = @line2, " +
                                     "update_datetime = @update_datetime;";

                        var cmd = new SQLiteCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@object_name", gpData.OBJECT_NAME);
                        cmd.Parameters.AddWithValue("@object_id", gpData.OBJECT_ID);
                        cmd.Parameters.AddWithValue("@epoch", gpData.EPOCH);
                        cmd.Parameters.AddWithValue("@mean_motion", gpData.MEAN_MOTION);
                        cmd.Parameters.AddWithValue("@eccentricity", gpData.ECCENTRICITY);
                        cmd.Parameters.AddWithValue("@inclination", gpData.INCLINATION);
                        cmd.Parameters.AddWithValue("@ra_of_asc_node", gpData.RA_OF_ASC_NODE);
                        cmd.Parameters.AddWithValue("@arg_of_pericenter", gpData.ARG_OF_PERICENTER);
                        cmd.Parameters.AddWithValue("@mean_anomaly", gpData.MEAN_ANOMALY);
                        cmd.Parameters.AddWithValue("@ephemeris_type", gpData.EPHEMERIS_TYPE);
                        cmd.Parameters.AddWithValue("@classification_type", gpData.CLASSIFICATION_TYPE);
                        cmd.Parameters.AddWithValue("@norad_cat_id", gpData.NORAD_CAT_ID);
                        cmd.Parameters.AddWithValue("@element_set_no", gpData.ELEMENT_SET_NO);
                        cmd.Parameters.AddWithValue("@rev_at_epoch", gpData.REV_AT_EPOCH);
                        cmd.Parameters.AddWithValue("@bstar", gpData.BSTAR);
                        cmd.Parameters.AddWithValue("@mean_motion_dot", gpData.MEAN_MOTION_DOT);
                        cmd.Parameters.AddWithValue("@mean_motion_ddot", gpData.MEAN_MOTION_DDOT);
                        cmd.Parameters.AddWithValue("@line1", gpData.LINE1);
                        cmd.Parameters.AddWithValue("@line2", gpData.LINE2);
                        cmd.Parameters.AddWithValue("@update_datetime", gpData.UPDATE_DATETIME);
                        cmd.ExecuteNonQuery();
                    }

                    EndTransaction(conn);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        public bool SelectGpData(string norad_cat_id, out GpData gpData)
        {
            gpData = null;

            try
            {
                using (var conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "SELECT object_name, object_id, epoch, mean_motion, eccentricity, inclination, ra_of_asc_node, " +
                        "arg_of_pericenter, mean_anomaly, ephemeris_type, classification_type, norad_cat_id, element_set_no, rev_at_epoch, " +
                        "bstar, mean_motion_dot, mean_motion_ddot, line1, line2, update_datetime FROM gp_data " +
                        $"WHERE norad_cat_id = @norad_cat_id";

                    var cmd = new SQLiteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@norad_cat_id", norad_cat_id);

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            gpData = new GpData
                            {
                                OBJECT_NAME = rdr["object_name"].ToString(),
                                OBJECT_ID = rdr["object_id"].ToString(),
                                EPOCH = rdr["epoch"].ToString(),
                                MEAN_MOTION = rdr["mean_motion"].ToString(),
                                ECCENTRICITY = rdr["eccentricity"].ToString(),
                                INCLINATION = rdr["inclination"].ToString(),
                                RA_OF_ASC_NODE = rdr["ra_of_asc_node"].ToString(),
                                ARG_OF_PERICENTER = rdr["arg_of_pericenter"].ToString(),
                                MEAN_ANOMALY = rdr["mean_anomaly"].ToString(),
                                EPHEMERIS_TYPE = rdr["ephemeris_type"].ToString(),
                                CLASSIFICATION_TYPE = rdr["classification_type"].ToString(),
                                NORAD_CAT_ID = rdr["norad_cat_id"].ToString(),
                                ELEMENT_SET_NO = rdr["element_set_no"].ToString(),
                                REV_AT_EPOCH = rdr["rev_at_epoch"].ToString(),
                                BSTAR = rdr["bstar"].ToString(),
                                MEAN_MOTION_DOT = rdr["mean_motion_dot"].ToString(),
                                MEAN_MOTION_DDOT = rdr["mean_motion_ddot"].ToString(),
                                LINE1 = rdr["line1"].ToString(),
                                LINE2 = rdr["line2"].ToString(),
                                UPDATE_DATETIME = DateTime.Parse(rdr["update_datetime"].ToString())
                            };
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLiteLib - {ex.Message}");
            }

            return false;
        }

        private void BeginTransaction(SQLiteConnection conn)
        {
            string sql = "BEGIN TRANSACTION";

            var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        private void EndTransaction(SQLiteConnection conn)
        {
            string sql = "END TRANSACTION";

            var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
