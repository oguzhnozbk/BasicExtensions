using System;
using System.IO;

namespace BasicExtensions
{
    public static class FileExtensions
    {
        private const string _password = "Devlet-i Ebed-Müddet";
        private static string GetPath(this string startupPath, params string[] files)
        {
            var path = $@"{startupPath}\Settings";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (files != null && files.Length > 0)
            {
                foreach (var item in files)
                {
                    path += $@"\{item}";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
            }
            return path;
        }
        public static void SaveSettings<T>(this T setting, string startupPath, params string[] files)
        {
            var path = $@"{startupPath.GetPath(files)}\{typeof(T).Name}.dem";
            if (File.Exists(path))
                File.Delete(path);
            using (var f = File.Create(path)) { }
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    var result = setting.ToJson();
                    writer.Write(result.EncryptText(_password));
                    writer.Flush();
                }
            }
        }
        public static T LoadSettings<T>(string startupPath, params string[] files)
        {
            var path = $@"{startupPath.GetPath(files)}\{typeof(T).Name}.dem";
            if (!File.Exists(path))
            {
                return default;
            }
            T settings;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    settings = reader.ReadToEnd().DecryptText(_password).ToModelFromJson<T>();
                }
            }
            return settings;
        }
        public static DateTime? GetLastWriteDate<T>(string startupPath, params string[] files)
        {
            var path = $@"{startupPath.GetPath(files)}\{typeof(T).Name}.dem";
            return File.Exists(path) ? File.GetLastWriteTime(path) : (DateTime?)null;
        }
    }
}
