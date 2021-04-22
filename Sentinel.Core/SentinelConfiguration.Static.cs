using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Sentinel.Core
{
    public partial class SentinelConfiguration
    {
        private SentinelConfiguration()
        {

        }

        public static SentinelConfiguration LoadFromFile(string path)
        {
            var fileContents = File.ReadAllText(path);

            var deserializer = new DeserializerBuilder().Build();

            return deserializer.Deserialize<SentinelConfiguration>(fileContents);
        }

        public static void SaveDefaultConfigToFile(string path)
        {
            var defaultConfig = new SentinelConfiguration()
            {

            };

            var serializer = new SerializerBuilder().Build();

            using (var writer = new StreamWriter(new FileStream(path, FileMode.Truncate)))
            {
                serializer.Serialize(writer, defaultConfig);
            }
        }
    }
}
