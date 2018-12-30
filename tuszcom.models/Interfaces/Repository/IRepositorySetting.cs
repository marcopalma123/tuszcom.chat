using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.models.DAO;

namespace tuszcom.models.Interfaces.Repository
{
    public interface IRepositorySetting
    {
        /// <summary>
        /// Dodawanie nowego ustawienia
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        bool AddSetting(string key, string value, string group, string description);
        /// <summary>
        /// Aktualizacja ustawienia
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool UpdateSetting(string key, string value, int id, string description);
        /// <summary>
        /// Usuwanie ustawienia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveSetting(int id);

        /// <summary>
        /// Zwraca wszystkie grupy ustawień
        /// </summary>
        /// <returns></returns>
        List<string> GetAllGroup();
        /// <summary>
        /// Pobieranie ustawienia po jego id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Settings GetSettingById(int id);

        /// <summary>
        /// Sprawdzanie czy klucz istnieje w bazie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExistsKeyInSettings(string key, int? id, string group);

        /// <summary>
        /// Pobranie wszystkich ustawień
        /// </summary>
        /// <returns></returns>
        List<Settings> GetAllSettings();
        /// <summary>
        /// Pobieranie wartości ustawienia po grupie i kluczu 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValueByGroupAndKey(string group, string key);

    }
}
