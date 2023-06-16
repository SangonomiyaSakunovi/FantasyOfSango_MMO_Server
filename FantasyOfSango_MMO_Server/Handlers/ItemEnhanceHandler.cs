using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Enums;
using FantasyOfSango_MMO_Server.Systems;
using SangoIOCPNet;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class ItemEnhanceHandler : BaseHandler
    {
        public ItemEnhanceHandler() { NetOpCode = OperationCode.ItemEnhance; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            if (peer.PeerEnhanceModeCode == PeerEnhanceModeCode.Done)
            {
                peer.SetPeerEnhanceModeCode(PeerEnhanceModeCode.Running);
                ItemEnhanceRsp itemEnhanceRsp = null;
                string itemEnhanceRequestJson = sangoNetMessage.MessageBody.MessageString;
                ItemEnhanceReq itemEnhanceReq = DeJsonString<ItemEnhanceReq>(itemEnhanceRequestJson);
                switch (itemEnhanceReq.ItemTypeCode)
                {
                    case ItemTypeCode.Weapon:
                        itemEnhanceRsp = ItemEnhanceSystem.Instance.GetWeaponEnhanceOrBreakResult(itemEnhanceReq, peer);
                        break;
                }
                string itemEnhanceResponseJson = SetJsonString(itemEnhanceRsp);
                peer.SendOperationResponse(NetOpCode, itemEnhanceResponseJson);
                peer.SetPeerEnhanceModeCode(PeerEnhanceModeCode.Done);
                if (!itemEnhanceRsp.IsEnhanceSuccess)
                {
                    IOCPLog.Warning("疑似用户作弊，对应Id为：" + peer.Account);
                }
            }
            else
            {
                IOCPLog.Warning("疑似用户攻击服务器，对应Id为：" + peer.Account);
            }
        }
    }
}
