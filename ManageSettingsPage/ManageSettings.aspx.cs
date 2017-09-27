using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Web.Configuration;
using Microsoft.SharePoint;
using System.Configuration;
using DevelopmentSimplyPut.CommonUtilities;
using DevelopmentSimplyPut.CommonUtilities.Logging;
using DevelopmentSimplyPut.CommonUtilities.Settings;
using DevelopmentSimplyPut.CommonUtilities.Security;
using DevelopmentSimplyPut.CommonUtilities.Helpers;
using System.Drawing;
using System.Data;
using DevelopmentSimplyPut.Entities;
using DevelopmentSimplyPut.BusinessLayer;
using DevelopmentSimplyPut.CommonUtilities.WebControls;
using System.Collections.ObjectModel;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace DevelopmentSimplyPut.Pages
{
    public partial class ManageSettings : BasePage
    {
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (null != Session["SettingsGridDataSource"])
			{
				List<SettingToken> lst = new List<SettingToken>();
				List<SettingToken> source = (List<SettingToken>)Session["SettingsGridDataSource"];
				foreach (GridViewRow row in grd_Settings.Rows)
				{
					if (row.RowType == DataControlRowType.DataRow)
					{
						SettingToken token = source[row.RowIndex];
						string newValue = ((TextBox)row.FindControl("txt_Value")).Text;
						CustomValidator validatior = (CustomValidator)row.FindControl("vld_txt_Value");

						if (!token.SettingDefinition.Validator(newValue))
						{
							token.ShowHint = true;
							validatior.IsValid = false;
							validatior.ErrorMessage = token.SettingDefinition.Hint;
						}
						else
						{
							token.ShowHint = false;
							validatior.IsValid = true;
							lst.Add(token);
						}

						token.Value = newValue;
					}
				}

				SystemSettingsProvider.UpdateSettings(lst);
			}
        }
        #endregion
        #region Methods
        private void BindData()
        {
            try
            {
                SystemLogger.Logger.LogMethodStart("BindData()", null, null);

                List<SettingToken> settings = GetSettinngs();

                if (null != settings && settings.Count > 0)
                {
                    grd_Settings.Visible = true;
                    SettingsGridDiv.Visible = true;
                    grd_Settings.PageIndex = 0;
                    grd_Settings.VirtualItemCount = settings.Count;
                    Session["SettingsGridDataSource"] = settings;
                    grd_Settings.DataSource = settings;
                    grd_Settings.DataBind();
                }
                else
                {
                    SettingsGridDiv.Visible = false;
                    grd_Settings.Visible = false;
                }

                SystemLogger.Logger.LogMethodEnd("BindData()", true);
            }
            catch (Exception ex)
            {
                SystemLogger.Logger.LogError(ex.Message);
                SystemLogger.Logger.LogMethodEnd("BindData()", false);
                SystemErrorHandler.HandleError(ex);
            }
        }
        private List<SettingToken> GetSettinngs()
        {
            List<SettingToken> result = new List<SettingToken>();

            foreach (SettingCatalogToken token in SystemSettingsCatalog.SettingsCatalog)
            {
                string value = SystemSettingsProvider.TryGetSettingValue(token.Category, token.Key);
                value = (string.IsNullOrEmpty(value)) ? string.Empty : value;

                SettingToken finalToken = new SettingToken();
                finalToken.SettingDefinition = token;
                finalToken.Value = value;

                if (!token.Validator(value))
                {
                    finalToken.ShowHint = true;
                }
                else
                {
                    finalToken.ShowHint = false;
                }

                result.Add(finalToken);
            }

            return result;
        }
        #endregion
    }
}
