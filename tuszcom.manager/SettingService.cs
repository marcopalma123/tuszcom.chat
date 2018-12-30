using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.dao.Repository;
using tuszcom.models.DAO;
using tuszcom.models.Interfaces.Services;

namespace tuszcom.services
{
    public class SettingService : IServiceSetting
    {
        private readonly SettingRepository repository;
        public SettingService()
        {
            repository = new SettingRepository();
        }
        public bool AddSetting(string key, string value, string group, string description)
        {
            return repository.AddSetting(key, value, group, description);
        }

        public List<string> GetAllGroup()
        {
            return repository.GetAllGroup();
        }

        public List<Settings> GetAllSettings()
        {
            return repository.GetAllSettings();
        }

        public Settings GetSettingById(int id)
        {
            return repository.GetSettingById(id);
        }

        public string GetValueByGroupAndKey(string group, string key)
        {
            return repository.GetValueByGroupAndKey(group, key);
        }

        public bool IsExistsKeyInSettings(string key, int? id, string group)
        {
            return repository.IsExistsKeyInSettings(key, id, group);
        }

        public bool RemoveSetting(int id)
        {
            return repository.RemoveSetting(id);
        }

        public bool UpdateSetting(string key, string value, int id, string description)
        {
            return repository.UpdateSetting(key, value, id, description);
        }
    }
}
