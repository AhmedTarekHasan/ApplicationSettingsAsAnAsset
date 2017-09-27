using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DevelopmentSimplyPut.CommonUtilities.Helpers;

namespace DevelopmentSimplyPut.CommonUtilities.Settings
{
	public enum BusinessSetting
    {
        DBConnectionString = 0,
        AdminsGroupName = 1,
		GridPageSize = 2,
        AutoCompleteMinCharCount = 3
    }

    public static class SystemSettingsCatalog
    {
        #region Constructor
        static SystemSettingsCatalog()
        {
            settingsCatalog = new Collection<SettingCatalogToken>();
            
			settingsCatalog.Add(new SettingCatalogToken()
            {
                BusinessSettingName = BusinessSetting.DBConnectionString,
                Category = "DevelopmentSimplyPut",
                Key = "DBConnectionString",
                Description = "Connection string of the SQL database",
                DefaultValue = string.Empty,
                Mandatory = true,
                RequiresIISReset = true,
                Hint = "Should be a valid ConnectionString of an online SQL database",
                Validator = new Func<string, bool>
                    (
                        delegate(string settingValue)
                        {
                            string exceptionMessage;
                            return Utilities.VerifySQLConnectionString(settingValue, out exceptionMessage);
                        }
                    ),
                Converter = new Func<string, object>
                    (
                        delegate(string settingValue)
                        {
                            return settingValue;
                        }
                    )
            });
            settingsCatalog.Add(new SettingCatalogToken()
            {
                BusinessSettingName = BusinessSetting.AdminsGroupName,
                Category = "DevelopmentSimplyPut",
                Key = "AdminsGroupName",
                Description = "Name of the Admins users group(s). Users in this/these group(s) will be allowed to access administration pages. Multiple group names should be separated by \",\"",
                DefaultValue = "AdminGroup",
                Mandatory = false,
                RequiresIISReset = false,
                Hint = string.Empty,
                Validator = new Func<string, bool>
                    (
                        delegate(string settingValue)
                        {
                            return true;
                        }
                    ),
                Converter = new Func<string, object>
                    (
                        delegate(string settingValue)
                        {
                            return settingValue;
                        }
                    )
            });
            settingsCatalog.Add(new SettingCatalogToken()
            {
                BusinessSettingName = BusinessSetting.GridPageSize,
                Category = "DevelopmentSimplyPut",
                Key = "GridPageSize",
                Description = "Number of items to be viewed in each page of the system data grids",
                DefaultValue = "20",
                RequiresIISReset = false,
                Mandatory = false,
                Hint = "Should be an integer greater than 0",
                Validator = new Func<string, bool>
                    (
                        delegate(string settingValue)
                        {
                            int result = 0;
                            return (int.TryParse(settingValue, out result) && result > 0);
                        }
                    ),
                Converter = new Func<string, object>
                    (
                        delegate(string settingValue)
                        {
                            return int.Parse(settingValue);
                        }
                    )
            });
            settingsCatalog.Add(new SettingCatalogToken()
            {
                BusinessSettingName = BusinessSetting.AutoCompleteMinCharCount,
                Category = "DevelopmentSimplyPut",
                Key = "AutoCompleteMinCharCount",
                Description = "The minimum number of characters to be entered into the textbox input fields for the auto-complete functionality to start. Please note that the chosen value will affect the system performance, so try to choose a number as large as you can",
                DefaultValue = "10",
                Mandatory = false,
                RequiresIISReset = false,
                Hint = "Should be an integer greater than 0",
                Validator = new Func<string, bool>
                    (
                        delegate(string settingValue)
                        {
                            int result = 0;
                            return (int.TryParse(settingValue, out result) && result > 0);
                        }
                    ),
                Converter = new Func<string, object>
                    (
                        delegate(string settingValue)
                        {
                            return int.Parse(settingValue);
                        }
                    )
            });
        }
        #endregion

        #region SettingsCatalog
        private static Collection<SettingCatalogToken> settingsCatalog;
        public static Collection<SettingCatalogToken> SettingsCatalog
        {
            get
            {
                return settingsCatalog;
            }
        }
        #endregion
    }
}
