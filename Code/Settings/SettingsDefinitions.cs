using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelopmentSimplyPut.CommonUtilities.Settings
{
    [Serializable]
    public class SettingCatalogToken
    {
        public BusinessSetting BusinessSettingName { set; get; }
        public string Category { set; get; }
        public string Key { set; get; }
        public string Description { set; get; }
        public bool Mandatory { set; get; }
        public string DefaultValue { set; get; }
        public string Hint { set; get; }
        public bool RequiresIISReset { set; get; }
        public Func<string, bool> Validator { set; get; }
        public Func<string, object> Converter { set; get; }
    }

    [Serializable]
    public class SettingToken
    {
        public SettingCatalogToken SettingDefinition { set; get; }
        public string Value { set; get; }
        public bool ShowHint { set; get; }
        public SettingToken() { }

        public SettingToken(SettingCatalogToken settingDefinition, string value)
        {
            SettingDefinition = settingDefinition;
            Value = value;
        }
    }

	public enum SettingsProviderType
    {
        ConfigStore = 0,
        WebConfig = 1
    }
	
    public interface ISettingsProvider
    {
        string GetSettingValue(string category, string key);
        void AddSettings(List<SettingToken> entries);
    }

    public class SettingsProviderFactory
    {
        public static ISettingsProvider GetProvider(SettingsProviderType providerType)
        {
            ISettingsProvider provider;

            switch (providerType)
            {
                case SettingsProviderType.ConfigStore:
                    provider = new ConfigStoreSettingsProvider();
                    break;
                case SettingsProviderType.WebConfig:
                    provider = new WebConfigSettingsProvider();
                    break;
                default:
                    provider = GetProvider(InternalConstants.DefaultSystemSettingsProvider);
                    break;
            }

            return provider;
        }
    } 
}
