using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class SyncPlayerAccountHandler : BaseHandler
    {
        public SyncPlayerAccountHandler() { NetOpCode = OperationCode.SyncPlayerAccount; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            List<string> onlineAccountList = OnlineAccountCache.Instance.GetSurroundAOIAccountList(peer);
            string onlineAccountJson = SetJsonString(onlineAccountList);
            peer.SendOperationResponse(NetOpCode, onlineAccountJson);
            NewAccountOnline newAccountOnline = new()
            {
                Account = peer.Account,
                AvaterCode = peer.CurrentAvater
            };
            string newAccountOnlineJson = SetJsonString(newAccountOnline);
            List<ClientPeer> onlinePeerList = OnlineAccountCache.Instance.GetSurroundAOIClientPeerList(peer);
            foreach (ClientPeer onlinePeer in onlinePeerList)
            {
                onlinePeer.SendEvent(NetOpCode, newAccountOnlineJson);
            }
        }
    }
}
