namespace System.Text.Json.Serialization
{
    public class JsonReadOnlyDictionaryAttribute : JsonConverterAttribute
    {
        public JsonReadOnlyDictionaryAttribute() : base(typeof(JsonReadOnlyDictionaryConverter))
        {
        }
    }
}