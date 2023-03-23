using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ClpCash.Infraestructure
{
    public class SingleConnection
    {
        private static SqlConnection Con = null;

        public static SqlConnection GetConnection()
        {
            try
            {
                string ConnectionString = ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                if (Con != null)
                {
                    if (Con.State != ConnectionState.Open)
                    {
                        Con = new SqlConnection(ConnectionString);
                        Con.Open();

                        return Con;
                    }

                    else return Con;
                }

                Con = new SqlConnection(ConnectionString);
                Con.Open();
                return Con;
            }
            catch (Exception e)
            {
                EventLog InsereLog = new EventLog();
                InsereLog.Source = "Serviço Integração Cash X CLP";
                InsereLog.WriteEntry(e.Message);
                InsereLog.Dispose();

                return Con;
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                {
                    Con.Close();
                }

            }
            catch (Exception e)
            {
                EventLog InsereLog = new EventLog();
                InsereLog.Source = "Serviço Integração Cash X CLP";
                InsereLog.WriteEntry(e.Message);
                InsereLog.Dispose();
            }
        }
    }
}
