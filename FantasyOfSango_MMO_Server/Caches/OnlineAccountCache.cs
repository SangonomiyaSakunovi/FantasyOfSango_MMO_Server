using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;
using System.Collections.Concurrent;
using System.Collections.Generic;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Caches
{
    public class OnlineAccountCache : BaseCache
    {
        public static OnlineAccountCache Instance = null;

        private ConcurrentDictionary<AOISceneGrid, List<string>> AOIAccountDict = new ConcurrentDictionary<AOISceneGrid, List<string>>();
        private ConcurrentDictionary<string, ClientPeer> OnlineAccountDict = new ConcurrentDictionary<string, ClientPeer>();

        public override void InitCache()
        {
            base.InitCache();
            Instance = this;
        }

        #region LookUpAOIAccount        
        public List<ClientPeer> GetCurrentAOIClientPeerList(NPCGameObject npcGameObject)
        {
            List<ClientPeer> aoiClientPeerList = new List<ClientPeer>();
            if (AOIAccountDict.ContainsKey(npcGameObject.AOISceneGrid))
            {
                List<string> tempList = DictTool.GetDictValue<AOISceneGrid, List<string>>(npcGameObject.AOISceneGrid, AOIAccountDict);
                if (tempList != null)
                {
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        ClientPeer tempPeer = GetOnlinePlayerClientPeerByAccount(tempList[j]);
                        aoiClientPeerList.Add(tempPeer);
                    }
                }
            }
            return aoiClientPeerList;
        }

        public List<string> GetSurroundAOIAccountList(ClientPeer clientPeer)
        {
            List<AOISceneGrid> surroundAOIGridList = AOISystem.Instance.GetSurroundAOIGrid(clientPeer.AOISceneGrid);
            List<string> aoiAccountList = new List<string>();
            for (int i = 0; i < surroundAOIGridList.Count; i++)
            {
                if (AOIAccountDict.ContainsKey(surroundAOIGridList[i]))
                {
                    List<string> tempList = DictTool.GetDictValue<AOISceneGrid, List<string>>(surroundAOIGridList[i], AOIAccountDict);
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

        public List<ClientPeer> GetSurroundAOIClientPeerList(ClientPeer clientPeer)
        {
            List<AOISceneGrid> surroundAOIGridList = AOISystem.Instance.GetSurroundAOIGrid(clientPeer.AOISceneGrid);
            List<ClientPeer> aoiClientPeerList = new List<ClientPeer>();
            for (int i = 0; i < surroundAOIGridList.Count; i++)
            {
                if (AOIAccountDict.ContainsKey(surroundAOIGridList[i]))
                {
                    List<string> tempList = DictTool.GetDictValue<AOISceneGrid, List<string>>(surroundAOIGridList[i], AOIAccountDict);
                    if (tempList != null)
                    {
                        for (int j = 0; j < tempList.Count; j++)
                        {
                            ClientPeer tempPeer = GetOnlinePlayerClientPeerByAccount(tempList[j]);
                            aoiClientPeerList.Add(tempPeer);
                        }
                    }
                }
            }
            return aoiClientPeerList;
        }
        #endregion

        #region UpdateAOIAccount
        public void UpdateOnlineAccountAOIInfo(string account, SceneCode sceneCode, float x, float z)
        {
            try
            {
                AOISceneGrid aoiSceneGridCurrent = DictTool.GetDictValue<string, ClientPeer>(account, OnlineAccountDict).AOISceneGrid;
                AOISceneGrid aoiSceneGridTemp = AOISystem.Instance.SetAOIGrid(sceneCode, x, z);
                if (aoiSceneGridTemp != aoiSceneGridCurrent)
                {
                    SetOnlineAccountAOISceneGrid(account, aoiSceneGridTemp);
                    AddOrUpdateAOIAccountDict(account, aoiSceneGridTemp);
                    RemoveAOIAccountDict(account, aoiSceneGridCurrent);
                }
            }
            catch { }
        }

        private void SetOnlineAccountAOISceneGrid(string account, AOISceneGrid aoiSceneGrid)
        {
            DictTool.GetDictValue<string, ClientPeer>(account, OnlineAccountDict).SetAOIGrid(aoiSceneGrid);
        }

        private void AddOrUpdateAOIAccountDict(string account, AOISceneGrid aoiSceneGrid)
        {
            if (AOIAccountDict.ContainsKey(aoiSceneGrid))
            {
                AOIAccountDict[aoiSceneGrid].Add(account);
            }
            else
            {
                AOIAccountDict.TryAdd(aoiSceneGrid, new List<string> { account });
            }
        }

        private void RemoveAOIAccountDict(string account, AOISceneGrid aoiSceneGrid)
        {
            if (AOIAccountDict.ContainsKey(aoiSceneGrid))
            {
                if (AOIAccountDict[aoiSceneGrid].Contains(account))
                {
                    AOIAccountDict[aoiSceneGrid].Remove(account);
                }
            }
        }
        #endregion

        public void AddOnlineAccount(ClientPeer clientPeer, string account)
        {
            OnlineAccountDict.TryAdd(account, clientPeer);
        }

        public bool IsAccountOnline(string account)
        {
            return OnlineAccountDict.ContainsKey(account);
        }

        public void RemoveOnlineAccount(ClientPeer clientPeer)
        {
            try
            {
                if (AOIAccountDict.ContainsKey(OnlineAccountDict[clientPeer.Account].AOISceneGrid))
                {
                    AOIAccountDict[OnlineAccountDict[clientPeer.Account].AOISceneGrid].Remove(clientPeer.Account);
                }
                if (OnlineAccountDict.ContainsKey(clientPeer.Account))
                {
                    OnlineAccountDict.TryRemove(clientPeer.Account, out ClientPeer peer);
                    if (peer == null)
                    {
                        Console.WriteLine("Remove OnlineAccountDict Wrong");
                    }
                }
            }
            catch { }            
        }

        public List<ClientPeer> GetAllOnlinePlayerClientPeerList()
        {
            List<ClientPeer> onlinePeerList = new List<ClientPeer>();
            foreach (var item in OnlineAccountDict.Values)
            {
                lock (item)
                {
                    onlinePeerList.Add(item);
                }
            }
            return onlinePeerList;
        }

        public ClientPeer GetOnlinePlayerClientPeerByAccount(string account)
        {
            return DictTool.GetDictValue<string, ClientPeer>(account, OnlineAccountDict);
        }
    }
}
