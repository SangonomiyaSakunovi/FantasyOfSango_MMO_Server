using SangoMMONetProtocol;
using System.Text.Json;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Bases
{
    public abstract class BaseHandler
    {
        public OperationCode NetOpCode;

        public abstract void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer);

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
