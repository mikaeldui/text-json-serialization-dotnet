using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.Generic.Tests
{
    [TestClass]
    public class ReadOnlyCollectionTests
    {
        [TestMethod]
        public void JsonDeserialisation()
        {
            const string jsonString = "[\"value1\",\"value2\"]";

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyCollectionConverter());
            var obj = JsonSerializer.Deserialize<ReadOnlyCollection<string>>(jsonString, options);

            Assert.IsNotNull(obj);

            Assert.AreEqual("value1", obj.First());
        }

        [TestMethod]
        public void JsonSerialisatoin()
        {
            var roo = Array.AsReadOnly(new string[] { "value1", "value2" });
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyCollectionConverter());
            var json = JsonSerializer.Serialize<ReadOnlyCollection<string>>(roo, options);

            Assert.AreEqual("[\"value1\",\"value2\"]", json);
        }
    }
}