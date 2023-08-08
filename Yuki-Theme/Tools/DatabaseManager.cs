using System.Collections.Generic;

namespace YukiTheme.Tools
{
    public class DatabaseManager
    {
        public static DatabaseManager Instance;

        private FileDatabase _database;

        public readonly Dictionary<int, string> _defaults = new()
        {
            { SettingsConst.AutoUpdate, true.ToInt().ToString() },
            { SettingsConst.BgImage, true.ToInt().ToString() },
            { SettingsConst.Sticker, true.ToInt().ToString() },
            { SettingsConst.StatusBar, true.ToInt().ToString() },
            { SettingsConst.Logo, true.ToInt().ToString() },
            { SettingsConst.Beta, true.ToInt().ToString() },
            { SettingsConst.Login, false.ToInt().ToString() },
            { SettingsConst.StickerPositionUnit, "1" },
            { SettingsConst.AllowPositioning, false.ToInt().ToString() },
            { SettingsConst.ShowGrids, false.ToInt().ToString() },
            { SettingsConst.UseCustomSticker, false.ToInt().ToString() },
            { SettingsConst.CustomStickerPath, "" },
            { SettingsConst.License, false.ToInt().ToString() },
            { SettingsConst.GoogleAnalytics, false.ToInt().ToString() },
            { SettingsConst.DonTTrack, false.ToInt().ToString() },
            { SettingsConst.AutoFitWidth, true.ToInt().ToString() },
            { SettingsConst.ShowPreview, true.ToInt().ToString() },
            { SettingsConst.Localization, "unknown" },
            { SettingsConst.UseDimensionCap, false.ToInt().ToString() },
            { SettingsConst.DimensionCapMax, "-1" },
            { SettingsConst.DimensionCapUnit, "0" },
            { SettingsConst.HideOnHover, true.ToInt().ToString() },
            { SettingsConst.HideDelay, "750" },
            { SettingsConst.PortableMode, false.ToInt().ToString() },
        };

        public DatabaseManager()
        {
            Instance = this;
            InitDatabase();

            AddDefaults();
        }

        private void InitDatabase()
        {
            _database = new FileDatabase();
        }

        private const string APP_NAME = "Yuki Theme";

        private void AddDefaults()
        {
            if (_database.GetValue(SettingsConst.AutoUpdate.ToString()).Length <= 2)
            {
                foreach (KeyValuePair<int, string> pair in _defaults)
                {
                    Save(pair.Key, pair.Value);
                }
            }
        }

        #region Setter

        public void Save(int name, int value) => Save(name.ToString(), value.ToString());
        public void Save(int name, bool value) => Save(name.ToString(), value.ToInt().ToString());

        public void Save(int name, string value)
        {
            Save(name.ToString(), value);
        }

        private void Save(string name, string value)
        {
            _database.SetValue(name, value);
        }

        #endregion

        #region Getter

        public string Load(int key, string defaultValue) => _database.GetValue(key.ToString(), defaultValue);

        public int Load(int key, int defaultValue)
        {
            return int.Parse(_database.GetValue(key.ToString(), defaultValue.ToString()));
        }

        public bool Load(int key, bool defaultValue)
        {
            return int.Parse(_database.GetValue(key.ToString(), defaultValue.ToInt().ToString())).ToBool();
        }

        private string Load(string key, string defaultValue) => _database.GetValue(key, defaultValue);

        

        #endregion


        public void Delete(int key)
        {
            _database.DeleteValue(key.ToString());
        }
    }
}