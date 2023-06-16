using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoIOCPNet;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class SyncPlayerTransformHandler : BaseHandler
    {
        public SyncPlayerTransformHandler() { NetOpCode = OperationCode.SyncPlayerTransform; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string playerTransformJson = sangoNetMessage.MessageBody.MessageString;
            TransformOnline playerTransform = DeJsonString<TransformOnline>(playerTransformJson);
            SyncPlayerTransformRsp syncPlayerTransformRsp = new();
            if (peer.TransformClock < 2)
            {
                peer.SetTransformOnline(playerTransform);
                OnlineAccountCache.Instance.UpdateOnlineAccountAOIInfo(peer.Account, SceneCode.Island, playerTransform.Vector3Position.X, playerTransform.Vector3Position.Z);
                syncPlayerTransformRsp.SyncPlayerTransformResult = true;
            }
            else
            {
                TransformOnline predictTransform = peer.CurrentTransformOnline;
                syncPlayerTransformRsp.SyncPlayerTransformResult = false;
                syncPlayerTransformRsp.PredictPlayerTransform = true;
                syncPlayerTransformRsp.TransformOnline = predictTransform;
            }
            peer.SetTransformClock(0);
            string syncPlayerTransformRspJson = SetJsonString(syncPlayerTransformRsp);
            peer.SendOperationResponse(NetOpCode, syncPlayerTransformRspJson);
        }
    }
}
