using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

namespace DevelopmentSimplyPut.CommonUtilities.Helpers
{
    public static class Utilities
    {
        /// <summary>
        /// Checks whether a given string represents a valid and online ConnectionString for a SQL database
        /// </summary>
        /// <param name="connectionString">String to be verified</param>
        /// <param name="exceptionMessage">Output exception message if exists</param>
        /// <returns></returns>
        public static bool VerifySQLConnectionString(string connectionString, out string exceptionMessage)
        {
            bool result;

            try
            {
                DbConnectionStringBuilder csb = new DbConnectionStringBuilder();
                csb.ConnectionString = connectionString;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                    }

                    exceptionMessage = null;
                    result = true;
                }
                catch(Exception ex)
                {
                    exceptionMessage = ex.Message;
                    result = false;
                }  
            }
            catch(Exception ex)
            {
                exceptionMessage = ex.Message;
                result = false;
            }

            return result;
        }
    }
}
