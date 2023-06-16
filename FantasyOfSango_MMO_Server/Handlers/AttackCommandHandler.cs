using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class AttackCommandHandler : BaseHandler
    {
        public AttackCommandHandler() { NetOpCode = OperationCode.AttackCommand; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string attackCommandJson = sangoNetMessage.MessageBody.MessageString;
            List<ClientPeer> aoiOnlinePeerList = OnlineAccountCache.Instance.GetSurroundAOIClientPeerList(peer);
            foreach (ClientPeer aoiOnlinePeer in aoiOnlinePeerList)
            {
                aoiOnlinePeer.SendEvent(NetOpCode, attackCommandJson);
            }
        }
    }
}
