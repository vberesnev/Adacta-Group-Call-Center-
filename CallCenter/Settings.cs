using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace CallCenter
{
    /// <summary>
    /// Класс настроек программы. Используется Newtonsoft для сериализации
    /// </summary>
    class Settings
    {
        private static string SETTINGS_PATH;
        
        public int OperatorsCount { get; set; } //Количество операторов
        public int ManagersCount { get; set; } //Количество менеджеров
        public int CallTimeStart { get; set; } //Время на один разговор от (в милисекундах!)
        public int CallTimeFinish { get; set; } //Время на один разговор до (в милисекундах!)
        public int CallRespawnStart { get; set; } //Время на создание нового разговора от (в милисекундах!)
        public int CallRespawmFinish { get; set; } //Время на создание нового разговора до (в милисекундах!)

        public Settings() { }

        public static Settings Load(string path)
        {
            SETTINGS_PATH = path;
            if (!File.Exists(SETTINGS_PATH))
            {
                return DefaultValues();
            }
            else
            {
                try
                {
                    string output = File.ReadAllText(SETTINGS_PATH, Encoding.UTF8);
                    Settings settings = JsonConvert.DeserializeObject<Settings>(output);
                    return settings;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при чтении настроек {ex.Message}");
                    Console.WriteLine("Установлены настройки по умолчанию");
                    return DefaultValues();
                }
            }
        }

        private static Settings DefaultValues()
        {
            Settings settings = new Settings()
            {
                OperatorsCount = 2,
                ManagersCount = 1,
                CallTimeStart = 3000,
                CallTimeFinish = 5000,
                CallRespawnStart = 500,
                CallRespawmFinish = 1000
            };

            var save = JsonConvert.SerializeObject(settings);
            File.WriteAllText(SETTINGS_PATH, save, Encoding.UTF8);
            return settings;
        }
    }
}
