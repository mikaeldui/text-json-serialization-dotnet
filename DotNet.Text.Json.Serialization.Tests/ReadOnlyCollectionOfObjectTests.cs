using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.Generic.Tests
{
    [TestClass]
    public class ReadOnlyCollectionOfObjectTests
    {
        [JsonReadOnlyCollection]
        public class TestCollection : ReadOnlyCollection<object>
        {
            public TestCollection(IList<object> list) : base(list)
            {
            }
        }

        [JsonReadOnlyCollection]
        public class TestCollection2<T> : ReadOnlyCollection<T>
        {
            public TestCollection2(IList<T> list) : base(list)
            {
            }
        }

        [TestMethod]
        public void JsonDeserialisation()
        {
            const string jsonString = "[\"value1\",\"value2\"]";

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyCollectionConverter());
            var obj = JsonSerializer.Deserialize<TestCollection2<object>>(jsonString, options);

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void JsonSerialisation()
        {
            var roo = Array.AsReadOnly(new object[] { "value1", "value2" });
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonReadOnlyCollectionConverter());
            var json = JsonSerializer.Serialize(roo, options);

            Assert.AreEqual("[\"value1\",\"value2\"]", json);
        }
    }
}