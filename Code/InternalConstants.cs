using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using DevelopmentSimplyPut.CommonUtilities.Settings;
using DevelopmentSimplyPut.CommonUtilities.Security;
using DevelopmentSimplyPut.CommonUtilities.Helpers;
using Microsoft.SharePoint;
using System.Collections.ObjectModel;

namespace DevelopmentSimplyPut.CommonUtilities
{
    public static class InternalConstants
    {
        #region Settings Provider
        public static SettingsProviderType DefaultSystemSettingsProvider
        {
            get
            {
                return SettingsProviderType.ConfigStore;
            }
        }
        public static string ConfigStoreListName
        {
            get
            {
                return "Config store";
            }
        }
		#endregion
    }
}
