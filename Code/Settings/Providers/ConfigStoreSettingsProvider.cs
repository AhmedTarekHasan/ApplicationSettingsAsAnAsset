using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using COB.SharePoint.Utilities;
using Microsoft.SharePoint;
using DevelopmentSimplyPut.CommonUtilities.Logging;

namespace DevelopmentSimplyPut.CommonUtilities.Settings
{
    public class ConfigStoreSettingsProvider : ISettingsProvider
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

            try
            {
                result = ConfigStore.GetValue(category, key);
                SystemLogger.Logger.LogMethodEnd("public string GetSettingValue(string category, string key)", true);
            }
            catch (Exception ex)
            {
                SystemLogger.Logger.LogError(ex, "Error in retrieving a setting value from config store list.");
                SystemLogger.Logger.LogMethodEnd("public string GetSettingValue(string category, string key)", false);
                throw;
            }

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
                    using (SPWeb web = site.RootWeb)
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;
                            SPList configStoreList = web.Lists[InternalConstants.ConfigStoreListName];

                            foreach (SettingToken token in entries)
                            {
                                SPQuery query = new SPQuery();
                                query.Query = string.Format
                                    (
                                        CultureInfo.InvariantCulture,
                                        @"<Where>
                                            <And>
                                                <Eq>
                                                    <FieldRef Name='{0}'/>
                                                    <Value Type='{1}'>{2}</Value>
                                                </Eq>
                                                <Eq>
                                                    <FieldRef Name='{3}'/>
                                                    <Value Type='{4}'>{5}</Value>
                                                </Eq>
                                            </And>
                                        </Where>",
                                        ConfigStore.CategoryField,
                                        "Text",
                                        token.SettingDefinition.Category,
                                        ConfigStore.KeyField,
                                        "Text",
                                        token.SettingDefinition.Key);

                                query.ViewFields = "<FieldRef Name='Title'/>";
                                query.RowLimit = 1;

                                SPListItemCollection items = configStoreList.GetItems(query);

                                if (null != items && items.Count > 0)
                                {
                                    string msg = "Setting with category = \"{0}\" and key = \"{1}\" already exists";
                                    SystemLogger.Logger.LogInfo
                                        (
                                            string.Format
                                                (
                                                    CultureInfo.InvariantCulture,
                                                    msg,
                                                    token.SettingDefinition.Category,
                                                    token.SettingDefinition.Key
                                                )
                                        );

                                    foreach (SPListItem item in items)
                                    {
                                        item[ConfigStore.ValueField] = token.Value;
                                        item["ConfigItemDescription"] = token.SettingDefinition.Description;
                                        item.Update();
                                    }
                                    configStoreList.Update();
                                    SystemLogger.Logger.LogInfo("Setting value is updated to \"" + token.Value + "\"");
                                }
                                else
                                {
                                    SPListItem entry = configStoreList.Items.Add();
                                    entry[ConfigStore.CategoryField] = token.SettingDefinition.Category;
                                    entry[ConfigStore.KeyField] = token.SettingDefinition.Key;
                                    entry[ConfigStore.ValueField] = token.Value;
                                    entry["ConfigItemDescription"] = token.SettingDefinition.Description;
                                    entry.SystemUpdate();
                                    configStoreList.Update();
                                }
                            }

                            SystemLogger.Logger.LogMethodEnd("public void AddSettings(SettingToken[] entries)", true);
                        }
                        catch (Exception ex)
                        {
                            SystemLogger.Logger.LogError(ex, "Error in adding setting entries in config store list.");
                            web.AllowUnsafeUpdates = false;
                            SystemLogger.Logger.LogMethodEnd("public void AddSettings(SettingToken[] entries)", false);
                            throw;
                        }
                    }
                }
            });
        }
    }
}
