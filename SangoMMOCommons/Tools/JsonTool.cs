using System.Text.Json;

//Developer: SangonomiyaSakunovi

namespace SangoMMOCommons.Tools
{
    public class JsonTool
    {
        public static string SetJsonString(object ob)
        {
            return JsonSerializer.Serialize(ob);
        }

        public static T DeJsonString<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str);
        }
    }
}
