using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.Generic.Tests
{
    [TestClass]
    public class ReadOnlyDictionaryTests
    {
        [TestMethod]
        public void JsonDeserialisation()
        {
            const string jsonString = "{\"key1\":\"value1\",\"key2\":\"value2\"}";

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyDictionaryConverter());
            var obj = JsonSerializer.Deserialize<ReadOnlyDictionary<string, string>>(jsonString, options);

            Assert.IsNotNull(obj);

            Assert.AreEqual("key1", obj.First().Key);
            Assert.AreEqual("value1", obj.First().Value);
        }

        [TestMethod]
        public void JsonSerialisation()
        {
            ReadOnlyDictionary<string, string> rod = new(new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } });
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyDictionaryConverter());
            var json = JsonSerializer.Serialize(rod, options);

            Assert.AreEqual("{\"key1\":\"value1\",\"key2\":\"value2\"}", json);
        }
    }
}