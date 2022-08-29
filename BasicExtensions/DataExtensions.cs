using BasicExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicExtensions
{
    public static class DataExtensions
    {
        public static string ToJson<T>(this T entity) => Newtonsoft.Json.JsonConvert.SerializeObject(entity);
        public static T ToModelFromJson<T>(this string entity) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(entity);
        public static string ToXml<T>(this T entity)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var xml = "";
            using (var stringWriter = new System.IO.StringWriter())
            {
                using (var xmlWriter = System.Xml.XmlWriter.Create(stringWriter))
                {
                    serializer.Serialize(xmlWriter, entity);
                    xml = stringWriter.ToString();
                }
            }
            return xml;
        }
        public static T ToModelFromXml<T>(this string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return default;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            T book;
            using (var reader = new System.IO.StringReader(data))
            {
                book = (T)(serializer.Deserialize(reader));
            }
            return book;
        }
        private static void Map<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (source != null && destination != null)
            {
                var sourceProperties = source.GetType().GetProperties().ToList();
                var destinationProperties = destination.GetType().GetProperties().ToList();

                foreach (var sourceProperty in sourceProperties)
                {
                    var destinationProperty = destinationProperties.Find(item => item.Name.ToLower() == sourceProperty.Name.ToLower());
                    if (destinationProperty != null)
                    {
                        try
                        {
                            destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
        public static TDestination Map<TDestination>(this object source)
            where TDestination : class, new()
        {
            var destination = Activator.CreateInstance<TDestination>();
            Map(source, destination);
            return destination;
        }
        public static TDestination MapJson<TDestination>(this object source) => source.ToJson().ToModelFromJson<TDestination>();
        public static T ToEnum<T>(this string val) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), val);
            }
            catch (Exception e)
            {
                return default;
            }
        }
        public static List<ListItem> ToList<T>() where T : Enum => EnumToList(typeof(T));
        public static List<ListItem> EnumToList(Type type) => Enum.GetValues(type).Cast<int>().Select(c => new ListItem() { Id = c.ToString(), Value = Enum.GetName(type, c) }).ToList();

    }
}
