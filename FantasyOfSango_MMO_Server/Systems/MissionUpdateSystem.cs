using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Configs;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class MissionUpdateSystem : BaseSystem
    {
        public static MissionUpdateSystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public MissionUpdateRsp GetMissionCompleteResult(MissionUpdateReq missionCompleteReq, ClientPeer clientPeer)
        {
            //TODO How to judge if this mission can complete?
            UpdateOnlineAccountMissionCompleteRewards(missionCompleteReq, clientPeer);
            MissionUpdateRsp missionUpdateRsp = new MissionUpdateRsp();
            missionUpdateRsp.MissionId = missionCompleteReq.MissionId;
            missionUpdateRsp.MissionUpdateTypeCode = MissionUpdateTypeCode.Complete;
            missionUpdateRsp.MissionTypeCode = MissionTypeCode.Main;
            missionUpdateRsp.IsCompleteSuccess = true;
            return missionUpdateRsp;
        }

        private void UpdateOnlineAccountMissionCompleteRewards(MissionUpdateReq missionCompleteReq, ClientPeer clientPeer)
        {
            MissionConfig missionConfig = resourceService.GetMissionConfig(missionCompleteReq.MissionId);
            clientPeer.ItemInfo.Coin += missionConfig.coinRewards;
        }
    }
}
