using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class ChooseAvaterHandler : BaseHandler
    {
        public ChooseAvaterHandler() { NetOpCode = OperationCode.ChooseAvater; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string chooseAvaterCodeJson = sangoNetMessage.MessageBody.MessageString;
            ChooseAvaterCode chooseAvaterCode = DeJsonString<ChooseAvaterCode>(chooseAvaterCodeJson);
            AvaterCode avater = chooseAvaterCode.AvaterCode;
            peer.SetCurrentAvaterIndexByAvaterCode(avater);
            List<ClientPeer> onlinePeerList = OnlineAccountCache.Instance.GetSurroundAOIClientPeerList(peer);
            foreach (ClientPeer onlinePeer in onlinePeerList)
            {
                onlinePeer.SendEvent(NetOpCode, chooseAvaterCodeJson);
            }
        }
    }
}
