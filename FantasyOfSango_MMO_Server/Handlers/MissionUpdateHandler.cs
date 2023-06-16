using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class MissionUpdateHandler : BaseHandler
    {
        public MissionUpdateHandler() { NetOpCode = OperationCode.MissionUpdate; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            MissionUpdateRsp missionUpdateRsp = null;
            string missionUpdateRequestJson = sangoNetMessage.MessageBody.MessageString;
            MissionUpdateReq missionUpdateReq = DeJsonString<MissionUpdateReq>(missionUpdateRequestJson);
            switch (missionUpdateReq.MissionUpdateTypeCode)
            {
                case MissionUpdateTypeCode.Complete:
                    missionUpdateRsp = MissionUpdateSystem.Instance.GetMissionCompleteResult(missionUpdateReq, peer);
                    break;
            }
            string missionUpdateRspJson = SetJsonString(missionUpdateRsp);
            peer.SendOperationResponse(NetOpCode, missionUpdateRspJson);
        }
    }
}
