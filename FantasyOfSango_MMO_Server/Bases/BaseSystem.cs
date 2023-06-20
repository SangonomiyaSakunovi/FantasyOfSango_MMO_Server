using FantasyOfSango_MMO_Server.Services;
using SangoMMONetProtocol;
using System.Text.Json;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Bases
{
    public class BaseSystem
    {
        protected ResourceService resourceService;

        public OperationCode NetOpCode;

        public virtual void InitSystem()
        {
            resourceService = ResourceService.Instance;
        }

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
