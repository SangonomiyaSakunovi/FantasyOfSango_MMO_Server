using FantasyOfSango_MMO_Server.Bases;
using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class SyncPlayerDataHandler : BaseHandler
    {
        public SyncPlayerDataHandler() { NetOpCode = OperationCode.SyncPlayerData; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            AvaterInfo avaterInfo = peer.AvaterInfo;
            MissionInfo missionInfo = peer.MissionInfo;
            ItemInfo itemInfo = peer.ItemInfo;
            PlayerData playerData = new()
            {
                AvaterInfo = avaterInfo,
                MissionInfo = missionInfo,
                ItemInfo = itemInfo
            };
            string playerDataJson = SetJsonString(playerData);
            peer.SendOperationResponse(NetOpCode, playerDataJson);
        }
    }
}
