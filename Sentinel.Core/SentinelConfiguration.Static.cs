using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sentinel.Core.Helpers;
using YamlDotNet.Serialization;

namespace Sentinel.Core
{
    public partial class SentinelConfiguration
    {
        private static SentinelConfiguration instance;

        public static SentinelConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = LoadFromFile();
                }

                return instance;
            }
        }


        private SentinelConfiguration()
        {

        }

        public static SentinelConfiguration LoadFromFile()
        {
            var path = HelperFunctions.FindExistingFile(new[]
            {
                "/etc/sentinel/conf.yml",
                "~/.config/sentinel/conf.yml",
                "C:\\projects\\sentinel\\conf.yml",
            }, "/etc/sentinel/conf.yml");

            var fileContents = File.ReadAllText(path);

            var deserializer = new DeserializerBuilder().Build();

            return deserializer.Deserialize<SentinelConfiguration>(fileContents);
        }

        public static void SaveDefaultConfigToFile(string path)
        {

            var defaultConfig = new SentinelConfiguration()
            {
                DatabaseProvider = "sqlite",
                DatabaseConnectionString = "Data Source=/etc/sentinel/conf.db",
                EnabledGenerators = new string[]
                {
                    "IPTablesPersistent",
                    "NetworkD"
                }
            };

            var serializer = new SerializerBuilder().Build();

            using (var writer = new StreamWriter(new FileStream(path, FileMode.Create)))
            {
                
                serializer.Serialize(writer, defaultConfig);
            }
        }
    }
}
