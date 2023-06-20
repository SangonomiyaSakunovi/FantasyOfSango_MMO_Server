using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Services;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Constants;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;
using System.Collections.Concurrent;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class SyncPlayerTransformSystem : BaseSystem
    {
        public static SyncPlayerTransformSystem Instance = null;
        private ConcurrentDictionary<AOISceneGrid, List<string>> AOIMovedAccountDict;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
            NetOpCode = OperationCode.SyncPlayerTransform;
            AOIMovedAccountDict = new ConcurrentDictionary<AOISceneGrid, List<string>>();
            TimerService.Instance.AddTickTask(200, SyncPlayerTransformCallBack, CallBack, 0);
        }

        private void SyncPlayerTransformCallBack(int taskId)
        {           
            CheckIfTheClientMoved();
            SendAOITransform();
        }

        private void CallBack(int taskId)
        {

        }

        private void CheckIfTheClientMoved()
        {
            AOIMovedAccountDict.Clear();
            List<ClientPeer> onlinePeerList = OnlineAccountCache.Instance.GetAllOnlinePlayerClientPeerList();
            for (int i = 0; i < onlinePeerList.Count; i++)
            {
                ClientPeer peer = onlinePeerList[i];
                if (peer.TransformClock == 0)
                {
                    if (peer.CurrentTransformOnline == null || peer.LastTransformOnline == null) continue;
                    if (Vector3Position.Distance(peer.CurrentTransformOnline.Vector3Position, peer.LastTransformOnline.Vector3Position) > ThreadsConstant.SyncPlayerTransformVector3PositionDistanceLimit)
                    {
                        SetAOIMovedAccountDict(peer);
                        SangoLog.LogTool.LogProcessing($"当前玩家: {peer.Account} ,位置发生了移动，坐标为: {peer.CurrentTransformOnline.Vector3Position.X+ ","+peer.CurrentTransformOnline.Vector3Position.Y + ", " + peer.CurrentTransformOnline.Vector3Position.Z}");
                    }
                }
                else
                {
                    if (peer.TransformClock > ThreadsConstant.SyncPlayerTransformClockMax)
                    {
                        //TODO
                        //We define in this occasion the client has disconnected
                    }
                    else
                    {
                        TransformOnline predictTransformOnline = PredictSystem.Instance.PredictNextTransform(peer.Account, peer.LastTransformOnline, peer.CurrentTransformOnline);
                        peer.SetTransformOnline(predictTransformOnline);
                        OnlineAccountCache.Instance.UpdateOnlineAccountAOIInfo(peer.Account, SceneCode.Island, predictTransformOnline.Vector3Position.X, predictTransformOnline.Vector3Position.Z);
                        if (Vector3Position.Distance(peer.CurrentTransformOnline.Vector3Position, peer.LastTransformOnline.Vector3Position) > ThreadsConstant.SyncPlayerTransformVector3PositionDistanceLimit)
                        {
                            SetAOIMovedAccountDict(peer);
                        }
                    }
                }
                peer.SetTransformClock(peer.TransformClock + 1);
            }
        }

        private void SendAOITransform()
        {
            List<ClientPeer> onlinePeerList = OnlineAccountCache.Instance.GetAllOnlinePlayerClientPeerList();
            for (int i = 0; i < onlinePeerList.Count; i++)
            {
                ClientPeer peer = onlinePeerList[i];
                AOISceneGrid aoiSceneGrid = peer.AOISceneGrid;
                List<TransformOnline> surroundAOIMovedTransformList = new List<TransformOnline>();
                if (aoiSceneGrid != null)
                {
                    List<string> surroundAOIMovedAccountList = GetSurroundAOIMovedAccount(aoiSceneGrid);
                    for (int j = 0; j < surroundAOIMovedAccountList.Count; j++)
                    {
                        TransformOnline aoiTransform = OnlineAccountCache.Instance.GetOnlinePlayerClientPeerByAccount(surroundAOIMovedAccountList[j]).CurrentTransformOnline;
                        surroundAOIMovedTransformList.Add(aoiTransform);
                    }
                }
                string surroundAOIMovedTransformListJson = SetJsonString(surroundAOIMovedTransformList);
                peer.SendEvent(NetOpCode, surroundAOIMovedTransformListJson);
            }
        }

        private void SetAOIMovedAccountDict(ClientPeer peer)
        {
            AOISceneGrid aoiCurrent = peer.AOISceneGrid;
            if (AOIMovedAccountDict.ContainsKey(aoiCurrent))
            {
                AOIMovedAccountDict[aoiCurrent].Add(peer.Account);
            }
            else
            {
                AOIMovedAccountDict.TryAdd(aoiCurrent, new List<string> { peer.Account });
            }
        }

        private List<string> GetSurroundAOIMovedAccount(AOISceneGrid aoiSceneGrid)
        {
            List<AOISceneGrid> surroundAOIGridList = AOISystem.Instance.GetSurroundAOIGrid(aoiSceneGrid);
            List<string> aoiAccountList = new List<string>();
            for (int i = 0; i < surroundAOIGridList.Count; i++)
            {
                if (AOIMovedAccountDict.ContainsKey(surroundAOIGridList[i]))
                {
                    List<string> tempList = DictTool.GetDictValue<AOISceneGrid, List<string>>(surroundAOIGridList[i], AOIMovedAccountDict);
                    if (tempList != null)
                    {
                        for (int j = 0; j < tempList.Count; j++)
                        {
                            aoiAccountList.Add(tempList[j]);
                        }
                    }
                }
            }
            return aoiAccountList;
        }       
    }
}
