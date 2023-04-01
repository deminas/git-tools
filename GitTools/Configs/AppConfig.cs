using System.IO;
using System.Reflection;
using System.Text.Json;

namespace GitTools.Configs
{
    internal class AppConfig
    {
        public readonly string _appFolder;

        public AppConfig()
        {
            var folderType = Environment.SpecialFolder.ApplicationData;
            var folder = Environment.GetFolderPath(folderType);
            var appName = Assembly.GetExecutingAssembly().GetName().Name ?? "GitTools";
            _appFolder = Path.Combine(folder, appName);

            if (!Directory.Exists(_appFolder))
            {
                Directory.CreateDirectory(_appFolder);
            }
        }

        private string GetFileName(string name)
        {
            return Path.Combine(_appFolder, $"{name}.json");
        }

        public void Save(string name, object data)
        {
            var fileName = GetFileName(name);

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
        }

        public T? Open<T>(string name)
            where T : class
        {
            var fileName = GetFileName(name);
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<T>(json);
            }
            return null;
        }
    }
}
