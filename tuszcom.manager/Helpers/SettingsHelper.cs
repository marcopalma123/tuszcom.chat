using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.models;

namespace tuszcom.services.Helpers
{
    public class SettingsHelper
    {
        public static string Get(string key)
        {
            try
            {
               var settingsManager = new SettingService();
                string[] parameters = key.Split(".");
                var value = settingsManager.GetValueByGroupAndKey(parameters[0], parameters[1]);
                return value;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return string.Empty;
            }
        }
    }
}
