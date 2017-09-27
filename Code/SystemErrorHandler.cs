using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using System.Globalization;
using System.Web.UI;
using System.Web;

namespace DevelopmentSimplyPut.CommonUtilities
{
    public static class SystemErrorHandler
    {
        public static void HandleError(Exception ex, string message)
        {
            string guid = System.Guid.NewGuid().ToString();
            SystemLogger.Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Unexpected error start, GUID = \"{0}\"", guid));

            if (null != ex)
            {
                SystemLogger.Logger.LogError(ex, message);
            }
            else
            {
                SystemLogger.Logger.LogError(message);
            }

            SystemLogger.Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Unexpected error end, GUID = \"{0}\"", guid));
            HttpContext.Current.Response.Redirect
                (
                    string.Format
                    (
                        CultureInfo.InvariantCulture,
                        "{0}/Error.aspx?generalmsg={1}&msg={2}&guid={3}",
                        InternalConstants.PagesDirectoryAbsolutePath,
                        HttpContext.Current.Server.UrlEncode(InternalConstants.UnexpectedErrorMsg),
                        HttpContext.Current.Server.UrlEncode(message),
                        HttpContext.Current.Server.UrlEncode(guid)
                    ), true
                );
        }
        public static void HandleError(Exception ex)
        {
            HandleError(ex, string.Empty);
        }
        public static void HandleError(string message)
        {
            HandleError(new Exception(".."), message);
        }
        public static void HandleError(string exceptionMessage, string message)
        {
            HandleError(new Exception(exceptionMessage), message);
        }
    }
}
