using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Text.Json.Serialization
{
    public class JsonReadOnlyDictionaryConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)))
                return false;

            if ((typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() != typeof(ReadOnlyDictionary<,>)) &&
                !typeof(ReadOnlyDictionary<,>).IsSubclassOfRawGeneric(typeToConvert))
                return false;

            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var iReadOnlyDictionary = typeToConvert.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>));
            Type keyType = iReadOnlyDictionary.GetGenericArguments()[0];
            Type valueType = iReadOnlyDictionary.GetGenericArguments()[1];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(IReadOnlyDictionaryConverterInner<,>).MakeGenericType(keyType, valueType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null, args: null, culture: null);

            return converter;
        }

        private class IReadOnlyDictionaryConverterInner<TKey, TValue> :
            JsonConverter<IReadOnlyDictionary<TKey, TValue>>
            where TKey : notnull
        {
            public override IReadOnlyDictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dictionary = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(ref reader, options: options);

                if (dictionary == null)
                    return null;

                return (IReadOnlyDictionary<TKey, TValue>)Activator.CreateInstance(
                    typeToConvert,
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { dictionary },
                    culture: null);
            }

            public override void Write(Utf8JsonWriter writer, IReadOnlyDictionary<TKey, TValue> dictionary, JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, dictionary, options);
        }
    }
}