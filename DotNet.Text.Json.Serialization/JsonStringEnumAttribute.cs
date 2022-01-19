using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Converts enumeration values to and from strings.
    /// </summary>
    public class JsonStringEnumAttribute : JsonConverterAttribute
    {
        public JsonStringEnumAttribute() : base(typeof(JsonStringEnumConverter))
        {
        }
    }
}