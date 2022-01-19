namespace System.Text.Json.Serialization
{
    public class JsonReadOnlyCollectionAttribute : JsonConverterAttribute
    {
        public JsonReadOnlyCollectionAttribute() : base(typeof(JsonReadOnlyCollectionConverter))
        {
        }
    }
}