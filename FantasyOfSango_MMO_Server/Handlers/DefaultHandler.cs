using FantasyOfSango_MMO_Server.Bases;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class DefaultHandler : BaseHandler
    {
        public DefaultHandler() { NetOpCode = OperationCode.Default; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {

        }
    }
}
