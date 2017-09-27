using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using DevelopmentSimplyPut.CommonUtilities.Helpers;

namespace DevelopmentSimplyPut.CommonUtilities.Settings
{
    public static class SystemSettingsProvider
    {
        private static ISettingsProvider provider;  
        public static T GetSettingValue<T>(BusinessSetting businessSettingName)
        {
            return GetSettingValue<T>(GetSettingCatalogToken(businessSettingName)); 
        }
        public static T GetSettingValue<T>(SettingCatalogToken settingCatalogToken)
        {
            string value = null;
            T result = default(T);

            if (null != settingCatalogToken)
            {
                try
                {
                    value = Provider.GetSettingValue(settingCatalogToken.Category, settingCatalogToken.Key);
                }
                catch (Exception ex)
                {
                    if (settingCatalogToken.Mandatory)
                    {
                        SystemErrorHandler.HandleError(ex, "\"" + settingCatalogToken.Key + "\" setting is not set");
                    }
                    else
                    {
                        value = settingCatalogToken.DefaultValue;
                    }
                }

                if (!settingCatalogToken.Validator(value))
                {
                    if (settingCatalogToken.Mandatory)
                    {
                        SystemErrorHandler.HandleError("Provided \"" + settingCatalogToken.Key + "\" setting is not valid");
                    }
                    else
                    {
                        value = settingCatalogToken.DefaultValue;
                    }
                }

                result = ((T)settingCatalogToken.Converter(value));
            }
            else
            {
                SystemErrorHandler.HandleError(new Exception("SettingToken is not provided into SettingsCatalog"));
            }

            return result;
        }
        public static string TryGetSettingValue(string category, string key)
        {
            string result = string.Empty;

            try
            {
                result = Provider.GetSettingValue(category, key);
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        public static bool UpdateSettings(List<SettingToken> settings)
        {
            bool result = true;
            List<SettingToken> toBeUpdated = new List<SettingToken>();

            if (null != settings && settings.Count > 0)
            {
                foreach (SettingToken token in settings)
                {
                    string value = token.Value;
                    if (!token.SettingDefinition.Validator(value))
                    {
                        token.ShowHint = true;
                        result = false;
                    }
                    else
                    {
                        token.ShowHint = false;
                        toBeUpdated.Add(token);
                    }
                }
            }

            Provider.AddSettings(toBeUpdated);

            return result;
        }
        public static SettingCatalogToken GetSettingCatalogToken(BusinessSetting businessSettingName)
        {
            return SystemSettingsCatalog.SettingsCatalog.DefaultIfEmpty(null).FirstOrDefault(setting => setting.BusinessSettingName == businessSettingName);
        }
        private static void SetProvider(SettingsProviderType providerType)
        {
            SystemLogger.Logger.LogMethodStart
                (
                    "private static void SetProvider(SettingsProviderType providerType)",
                    new string[] { "providerType" },
                    new object[] { providerType }
                );

            try
            {
                provider = SettingsProviderFactory.GetProvider(providerType);
                SystemLogger.Logger.LogInfo("SystemSetitngsProvider is set to " + providerType.ToString());
                SystemLogger.Logger.LogMethodEnd("private static void SetProvider(SettingsProviderType providerType)", true);
            }
            catch (Exception ex)
            {
                SystemLogger.Logger.LogError(ex, "Failed in setting SettingsProviderType");
                SystemLogger.Logger.LogMethodEnd("private static void SetProvider(SettingsProviderType providerType)", false);
                throw;
            }
        }
        private static ISettingsProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    SetProvider(InternalConstants.DefaultSystemSettingsProvider);
                }
                return provider;
            }
        }
        public static void ResetProvider()
        {
            ResetProvider(InternalConstants.DefaultSystemSettingsProvider);
        }
        public static void ResetProvider(SettingsProviderType providerType)
        {
            SetProvider(providerType);
        }
    }
}
