using SangoMMONetProtocol;
using System.Text.Json;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Bases
{
    public abstract class BaseThreads
    {
        public abstract void Run();

        public abstract void Update();

        public abstract void Stop();

        public OperationCode NetOpCode;

        protected static string SetJsonString(object ob)
        {
            return JsonSerializer.Serialize(ob);
        }

        protected static T DeJsonString<T>(string str)
        {
            return JsonSerializer.Deserialize<T>(str);
        }
    }
}
