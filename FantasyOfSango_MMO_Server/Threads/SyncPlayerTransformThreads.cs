using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Constants;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Threads
{
    public class SyncPlayerTransformThreads : BaseThreads
    {
        private Thread _thread;
        private Dictionary<AOISceneGrid, List<string>> AOIMovedAccountDict = new Dictionary<AOISceneGrid, List<string>>();

        public override void Run()
        {
            _thread = new Thread(Update);
            _thread.IsBackground = true;
            _thread.Start();
            NetOpCode = OperationCode.SyncPlayerTransform;
        }

        public override void Update()
        {
            Thread.Sleep(ThreadsConstant.SyncPlayerTransformSleep);
            while (true)
            {
                Thread.Sleep(ThreadsConstant.SyncPlayerTransformTime);
                CheckIfTheClientMoved();
                SendAOITransform();
            }
        }

        public override void Stop()
        {
            _thread.Abort();
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
                AOIMovedAccountDict.Add(aoiCurrent, new List<string> { peer.Account });
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
