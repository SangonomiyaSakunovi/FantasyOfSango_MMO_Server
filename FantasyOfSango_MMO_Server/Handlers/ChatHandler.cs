using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class ChatHandler : BaseHandler
    {
        public ChatHandler() { NetOpCode = OperationCode.Chat; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string onlineAccountChatMessageJson = sangoNetMessage.MessageBody.MessageString;
            peer.SendOperationResponse(NetOpCode, ReturnCode.Success);
            List<ClientPeer> allOnlinePeerList = OnlineAccountCache.Instance.GetAllOnlinePlayerClientPeerList();
            foreach (ClientPeer onlinePeer in allOnlinePeerList)
            {
                onlinePeer.SendEvent(NetOpCode, onlineAccountChatMessageJson);
            }
        }
    }
}
