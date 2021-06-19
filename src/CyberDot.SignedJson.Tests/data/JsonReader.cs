using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CyberDot.SignedJson.Tests
{
    public static class JsonReader
    {
        public static string ReadJson(string path)
        {
            var json = JObject.Parse(File.ReadAllText(path));
            return json.ToString(Formatting.None);
        }
    }
}