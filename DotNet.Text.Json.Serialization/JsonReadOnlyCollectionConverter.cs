using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Text.Json.Serialization
{
    public class JsonReadOnlyCollectionConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsArray)
                return false;

            if (!typeToConvert.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>)))
                return false;

            if ((typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() != typeof(ReadOnlyCollection<>)) &&
                !typeof(ReadOnlyCollection<>).IsSubclassOfRawGeneric(typeToConvert))
                return false;

            return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var iReadOnlyCollection = typeToConvert.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>));
            Type keyType = iReadOnlyCollection.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(IReadOnlyCollectionConverterInner<>).MakeGenericType(keyType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null, args: null, culture: null);

            return converter;
        }

        private class IReadOnlyCollectionConverterInner<TValue> :
            JsonConverter<IReadOnlyCollection<TValue>>
            where TValue : notnull
        {
            public override IReadOnlyCollection<TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var array = JsonSerializer.Deserialize<TValue[]>(ref reader, options: options);

                if (array == null)
                    return null;

                return (IReadOnlyCollection<TValue>)Activator.CreateInstance(
                    typeToConvert,
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { array },
                    culture: null);
            }

            public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<TValue> dictionary, JsonSerializerOptions options) =>
                JsonSerializer.Serialize(writer, dictionary, options);
        }
    }
}