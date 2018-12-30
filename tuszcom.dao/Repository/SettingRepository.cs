using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tuszcom.models;
using tuszcom.models.DAO;
using tuszcom.models.Interfaces.Repository;

namespace tuszcom.dao.Repository
{
    public class SettingRepository : IRepositorySetting
    {
        private ChatDbContext context;
        public SettingRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
            context = new ChatDbContext(optionsBuilder.Options);

        }

        public bool AddSetting(string key, string value, string group, string description)
        {
            try
            {
                Settings setting = new Settings();
                setting.Key = key;
                setting.Value = value;
                setting.Group = group;
                setting.AllowDelete = true;
                setting.Description = description;
                context.Settings.Add(setting);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public List<string> GetAllGroup()
        {
            try
            {
                var group = context.Settings.Select(x => new { group = x.Group }).ToList();

                List<string> result = new List<string>();

                foreach (var item in group)
                {
                    if (!result.Contains(item.group))
                        result.Add(item.group);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null; ;
            }
        }

        public List<Settings> GetAllSettings()
        {
            try
            {
                return context.Settings.ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null; ;
            }
        }

        public Settings GetSettingById(int id)
        {
            try
            {
                return context.Settings.Single(x => x.IdSetting == id);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null;
            }
        }

        public string GetValueByGroupAndKey(string group, string key)
        {
            try
            {
                return context.Settings.FirstOrDefault(x => x.Group == group && x.Key == key).Value;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null;
            }
        }

        public bool IsExistsKeyInSettings(string key, int? id, string group)
        {
            try
            {
                bool isExists;
                if (id != null)
                    isExists = context.Settings.Any(x => x.Key == key && x.IdSetting != Convert.ToInt32(id) && x.Group == group);
                else
                    isExists = context.Settings.Any(x => x.Key == key && x.Group == group);

                return isExists;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool RemoveSetting(int id)
        {
            try
            {
                var setting = context.Settings.Single(x => x.IdSetting == id);

                context.Settings.Remove(setting);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool UpdateSetting(string key, string value, int id, string description)
        {
            try
            {
                var setting = context.Settings.Single(x => x.IdSetting == id);
                setting.Value = value;
                setting.Key = key;
                setting.Description = description;
                context.Settings.Update(setting);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }
    }
}
