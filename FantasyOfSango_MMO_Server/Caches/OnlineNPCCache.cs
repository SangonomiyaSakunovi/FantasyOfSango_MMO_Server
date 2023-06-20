using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.FSMs;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;
using System.Collections.Concurrent;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Caches
{
    public class OnlineNPCCache : BaseCache
    {
        public static OnlineNPCCache Instance = null;

        private ConcurrentDictionary<AOISceneGrid, List<NPCCode>> AOINPCGameObjectDict = new ConcurrentDictionary<AOISceneGrid, List<NPCCode>>();
        private ConcurrentDictionary<NPCCode, NPCGameObject> NPCGameObjectDict = new ConcurrentDictionary<NPCCode, NPCGameObject>();
        private ConcurrentDictionary<NPCCode, FSMSystem> NPCFSMSystemDict = new ConcurrentDictionary<NPCCode, FSMSystem>();
        private ConcurrentDictionary<NPCCode, BaseAI> NPCAISystemDict = new ConcurrentDictionary<NPCCode, BaseAI>();

        public override void InitCache()
        {
            base.InitCache();
            Instance = this;
        }

        public void AddOrUpdateAOINPCGameObjectDict(NPCCode npcCode, AOISceneGrid aoiSceneGrid)
        {
            if (AOINPCGameObjectDict.ContainsKey(aoiSceneGrid))
            {
                AOINPCGameObjectDict[aoiSceneGrid].Add(npcCode);
            }
            else
            {
                AOINPCGameObjectDict.TryAdd(aoiSceneGrid, new List<NPCCode> { npcCode });
            }
        }

        public List<NPCCode> GetSurroundAOINPCGameObject(AOISceneGrid aoiSceneGrid)
        {
            List<AOISceneGrid> surroundAOIGridList = AOISystem.Instance.GetSurroundAOIGrid(aoiSceneGrid);
            List<NPCCode> aoiNPCList = new List<NPCCode>();
            List<NPCCode> tempList1 = DictTool.GetDictValue<AOISceneGrid, List<NPCCode>>(aoiSceneGrid, AOINPCGameObjectDict);
            if (tempList1 != null)
            {
                for (int i = 0; i < tempList1.Count; i++)
                {
                    aoiNPCList.Add(tempList1[i]);
                }
            }
            for (int j = 0; j < surroundAOIGridList.Count; j++)
            {
                if (AOINPCGameObjectDict.ContainsKey(surroundAOIGridList[j]))
                {
                    List<NPCCode> tempList2 = DictTool.GetDictValue<AOISceneGrid, List<NPCCode>>(surroundAOIGridList[j], AOINPCGameObjectDict);
                    if (tempList2 != null)
                    {
                        for (int k = 0; k < tempList2.Count; k++)
                        {
                            aoiNPCList.Add(tempList2[k]);
                        }
                    }
                }
            }
            return aoiNPCList;
        }

        public void AddNPCAISystem<T>(NPCCode npcCode, BaseAI ai) where T : BaseAI
        {
            if (ai is T subAI)
                NPCAISystemDict.TryAdd(npcCode, subAI);
        }

        public BaseAI GetNPCAI(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, BaseAI>(npcCode, NPCAISystemDict);
        }

        public void AddNPCGameObject(NPCCode npcCode, NPCGameObject npcGameObject)
        {
            NPCGameObjectDict.TryAdd(npcCode, npcGameObject);
        }

        public NPCGameObject GetNPCGameObject(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, NPCGameObject>(npcCode, NPCGameObjectDict);
        }

        public void AddNPCFSMSystem(NPCCode npcCode, FSMSystem fSMSystem)
        {
            NPCFSMSystemDict.TryAdd(npcCode, fSMSystem);
        }

        public FSMSystem GetNPCFSMSystem(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, FSMSystem>(npcCode, NPCFSMSystemDict);
        }

        public IEnumerable<BaseAI> GetNPCAIs() => NPCAISystemDict.Values;
    }
}
