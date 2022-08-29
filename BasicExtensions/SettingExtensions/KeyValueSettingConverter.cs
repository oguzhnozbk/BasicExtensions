using BasicExtensions.SettingExtensions.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BasicExtensions.SettingExtensions
{
    public static class KeyValueSettingConverter
    {
        public static Dictionary<string, string> ToKeyValuePairs<T>(this T settings) where T : ISetting => settings.Save(typeof(T), "");
        public static T ToModel<T>(this Dictionary<string, string> pairs) where T : ISetting
        {
            var instance = (T)Activator.CreateInstance(typeof(T));
            return Load(out instance, pairs, typeof(T), "");
        }
        private static Dictionary<string, string> Save<T>(this T settings, Type type = null, string pref = "") where T : ISetting
        {
            var result = new Dictionary<string, string>();
            if (settings != null)
            {
                if (type == null) type = typeof(T);
                foreach (var item in type.GetProperties())
                {
                    var p = string.IsNullOrWhiteSpace(pref) ? $"{typeof(ObjectAttribute).Name}.{type.Name}" : $"{pref}.{type.Name}";
                    p = p.ToLowerInvariant();
                    var attributes = item.GetCustomAttributes(false);
                    if (attributes.Any(s => s.GetType().Name == typeof(ListAttribute).Name))
                    {
                        p = $"{p}.{typeof(ListAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var val = item.GetValue(settings, null);
                        if (val != null)
                        {
                            var c = val as IList;
                            for (int i = 0; i < c?.Count; i++)
                            {
                                var r = ((ISetting)c[i]).Save(c[i].GetType(), $"{p}.{i}");
                                foreach (var d in r)
                                    result.Add(d.Key, d.Value);
                            }
                        }
                    }
                    else if (attributes.Any(s => s.GetType().Name == typeof(ObjectAttribute).Name))
                    {
                        p = $"{p}.{typeof(ObjectAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var val = (ISetting)item.GetValue(settings);
                        var r = val.Save(val?.GetType(), $"{p}");
                        foreach (var d in r)
                            result.Add(d.Key, d.Value);
                    }
                    else if (attributes.Any(s => s.GetType().Name == typeof(ValueAttribute).Name))
                    {
                        p = $"{p}.{typeof(ValueAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var val = item.GetValue(settings);
                        result.Add(p, val?.ToString());
                    }
                }
            }


            return result;
        }
        private static T Load<T>(out T instance, Dictionary<string, string> pairs, Type type = null, string pref = "") where T : ISetting
        {
            if (type == null) type = typeof(T);
            var settings = Activator.CreateInstance(type);
            if (pairs != null && pairs.Count > 0)
            {

                foreach (var item in type.GetProperties())
                {
                    var key = string.IsNullOrWhiteSpace(pref) ? $"{typeof(ObjectAttribute).Name}.{type.Name}" : $"{pref}.{type.Name}";
                    key = key.ToLowerInvariant();
                    var attributes = item.GetCustomAttributes(false);
                    if (attributes.Any(s => s.GetType().Name == typeof(ListAttribute).Name))
                    {
                        key = $"{key}.{typeof(ListAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var length = pairs.Where(s => s.Key.ToLowerInvariant().Contains(key)).Select(s => { var d = s.Key.ToLowerInvariant().Replace($"{key.ToLowerInvariant()}.", ""); d = d.Substring(0, d.IndexOf('.')); return d; }).Distinct().Count();
                        if (length > 0)
                        {
                            var result = Activator.CreateInstance(item.PropertyType);
                            for (int i = 0; i < length; i++)
                            {
                                var p = $"{key}.{i}".ToLowerInvariant();
                                var setting = pairs.Where(s => s.Key.ToLowerInvariant().Contains(p)).ToDictionary(c => c.Key.ToLowerInvariant(), c => c.Value);
                                var v = (ISetting)Activator.CreateInstance(item.PropertyType.GetGenericArguments()[0]);
                                Load(out v, setting, item.PropertyType.GetGenericArguments()[0], p);
                                ((IList)result).Add(v);
                            }
                            item.SetValue(settings, result, null);
                        }
                    }
                    else if (attributes.Any(s => s.GetType().Name == typeof(ObjectAttribute).Name))
                    {
                        key = $"{key}.{typeof(ObjectAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var setting = pairs.Where(s => s.Key.ToLowerInvariant().Contains(key)).ToDictionary(c => c.Key.ToLowerInvariant(), c => c.Value);
                        var v = (ISetting)Activator.CreateInstance(item.PropertyType);
                        Load(out v, setting, item.PropertyType, key);
                        item.SetValue(settings, v, null);
                    }
                    else if (attributes.Any(s => s.GetType().Name == typeof(ValueAttribute).Name))
                    {
                        key = $"{key}.{typeof(ValueAttribute).Name}.{item.Name}".ToLowerInvariant();
                        var v = pairs.Any(x => x.Key.ToLowerInvariant() == key) ? pairs.First(s => s.Key.ToLowerInvariant() == key).Value : null;
                        if (v != null)
                        {
                            var value = TypeDescriptor.GetConverter(item.PropertyType).ConvertFromInvariantString(v);
                            item.SetValue(settings, value, null);
                        }
                    }
                }
            }
            else
            {
                settings = default;
            }
            instance = (T)settings;
            return instance;
        }
    }
}
