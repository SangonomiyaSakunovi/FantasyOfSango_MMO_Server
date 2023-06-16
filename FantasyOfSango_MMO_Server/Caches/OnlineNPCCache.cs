using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.FSMs;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;
using SangoMMOCommons.Tools;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Caches
{
    public class OnlineNPCCache : BaseCache
    {
        public static OnlineNPCCache Instance = null;

        private Dictionary<AOISceneGrid, List<NPCCode>> AOINPCGameObjectDict = new Dictionary<AOISceneGrid, List<NPCCode>>();
        private Dictionary<NPCCode, NPCGameObject> NPCGameObjectDict = new Dictionary<NPCCode, NPCGameObject>();
        private Dictionary<NPCCode, FSMSystem> NPCFSMSystemDict = new Dictionary<NPCCode, FSMSystem>();
        private Dictionary<NPCCode, BaseAI> NPCAISystemDict = new Dictionary<NPCCode, BaseAI>();

        public override void InitCache()
        {
            base.InitCache();
            Instance = this;
        }

        public void AddOrUpdateAOINPCGameObjectDict(NPCCode npcCode, AOISceneGrid aoiSceneGrid, string id = "Test")
        {
            lock (id)
            {
                if (AOINPCGameObjectDict.ContainsKey(aoiSceneGrid))
                {
                    AOINPCGameObjectDict[aoiSceneGrid].Add(npcCode);
                }
                else
                {
                    AOINPCGameObjectDict.Add(aoiSceneGrid, new List<NPCCode> { npcCode });
                }
            }
        }

        public List<NPCCode> GetSurroundAOINPCGameObject(AOISceneGrid aoiSceneGrid, string id = "Test")
        {
            lock (id)
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
        }

        public void AddNPCAISystem<T>(NPCCode npcCode, BaseAI ai) where T : BaseAI
        {
            if (ai is T subAI)
                NPCAISystemDict.Add(npcCode, subAI);
        }

        public BaseAI GetNPCAI(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, BaseAI>(npcCode, NPCAISystemDict);
        }

        public void AddNPCGameObject(NPCCode npcCode, NPCGameObject npcGameObject)
        {
            NPCGameObjectDict.Add(npcCode, npcGameObject);
        }

        public NPCGameObject GetNPCGameObject(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, NPCGameObject>(npcCode, NPCGameObjectDict);
        }

        public void AddNPCFSMSystem(NPCCode npcCode, FSMSystem fSMSystem)
        {
            NPCFSMSystemDict.Add(npcCode, fSMSystem);
        }

        public FSMSystem GetNPCFSMSystem(NPCCode npcCode)
        {
            return DictTool.GetDictValue<NPCCode, FSMSystem>(npcCode, NPCFSMSystemDict);
        }

        public IEnumerable<BaseAI> GetNPCAIs() => NPCAISystemDict.Values;
    }
}
