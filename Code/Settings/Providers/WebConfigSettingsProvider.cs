using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.SharePoint.Administration;
using System.Globalization;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using System.Web.Configuration;
using Microsoft.SharePoint;

namespace DevelopmentSimplyPut.CommonUtilities.Settings
{
    public class WebConfigSettingsProvider : ISettingsProvider
    {
        public string GetSettingValue(string category, string key)
        {
            SystemLogger.Logger.LogMethodStart
                (
                    "public string GetSettingValue(string category, string key)",
                    new string[] { "category", "key" },
                    new object[] { category, key }
                );

            string result = string.Empty;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    try
                    {
                        Configuration config = WebConfigurationManager.OpenWebConfiguration("/", site.WebApplication.Name);
                        if (config.AppSettings.Settings[category.ToLower() + key.ToLower()] != null)
                        {
                            result = config.AppSettings.Settings[category.ToLower() + key.ToLower()].Value;
                            SystemLogger.Logger.LogMethodEnd("public string GetSettingValue(string category, string key)", true);
                        }
                    }
                    catch(Exception ex)
                    {
                        string msg = "Error in retrieveing a setting value from web.config file.";
                        SystemLogger.Logger.LogMethodEnd("public string GetSettingValue(string category, string key)", false);
                        SystemLogger.Logger.LogError(ex, msg);
                        throw;
                    }
                }
            });

            return result;
        }
        public void AddSettings(List<SettingToken> entries)
        {
            SystemLogger.Logger.LogMethodStart
                (
                    "public void AddSettings(SettingToken[] entries)",
                    new string[] { "entries" },
                    new object[] { entries }
                );

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.Url))
                {
                    try
                    {
                        SPWebService service = SPWebService.ContentService;

                        foreach (SettingToken token in entries)
                        {
                            SPWebConfigModification myModification = new SPWebConfigModification();
                            myModification.Path = "configuration/appSettings";
                            myModification.Name = string.Format(CultureInfo.InvariantCulture, "add[@key=\"{0}\"]", token.SettingDefinition.Category.ToLower() + token.SettingDefinition.Key.ToLower());
                            myModification.Sequence = 0;
                            myModification.Owner = "System";
                            myModification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
                            myModification.Value = string.Format(CultureInfo.InvariantCulture, "<add key=\"{0}\" value=\"{1}\"/>", token.SettingDefinition.Category.ToLower() + token.SettingDefinition.Key.ToLower(), token.Value);
                            site.WebApplication.WebConfigModifications.Add(myModification);
                            service.Update();
                            service.ApplyWebConfigModifications();
                        }

                        SystemLogger.Logger.LogMethodEnd("public void AddSettings(SettingToken[] entries)", true);
                    }
                    catch (Exception ex)
                    {
                        string msg = "Error in adding setting entries in web.config file.";
                        SystemLogger.Logger.LogError(ex, msg);
                        SystemLogger.Logger.LogMethodEnd("public void AddSettings(SettingToken[] entries)", false);
                        throw;
                    }
                }
            });
        }
    }
}
