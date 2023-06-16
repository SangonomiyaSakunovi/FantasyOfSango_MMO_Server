using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class AttackDamageHandler : BaseHandler
    {
        public AttackDamageHandler() { NetOpCode = OperationCode.AttackDamage; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string attackDamageJson = sangoNetMessage.MessageBody.MessageString;
            AttackDamage attackDamage = DeJsonString<AttackDamage>(attackDamageJson);

            AttackResult attackResult = OnlineAttackSystem.Instance.GetAttackResult(attackDamage);
            if (attackResult != null)
            {
                string attackResultJson = SetJsonString(attackResult);
                peer.SendOperationResponse(NetOpCode, attackResultJson);

                List<ClientPeer> onlinePeerList = OnlineAccountCache.Instance.GetSurroundAOIClientPeerList(peer);
                foreach (ClientPeer onlinePeer in onlinePeerList)
                {
                    onlinePeer.SendEvent(NetOpCode, attackResultJson);
                }
            }
            OnlineAttackCache.Instance.AddOnlineAttackDamageCache(attackDamage);
        }
    }
}
