using FantasyOfSango_MMO_Server.AIs;
using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.FSMs;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class NPCSystem : BaseSystem
    {
        public static NPCSystem Instance;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
            InitNPCGameObject();
        }

        private void InitNPCGameObject()
        {
            InitEnemyGameObject();
        }

        private void InitEnemyGameObject()
        {
            NPCGameObject testHilichurlGameObject = new NPCGameObject
            {
                _id = "testHilichurl", //here can be any name, no logic related
                NPCCode = NPCCode.Hilichurl_Island_A_01,
                Vector3Position = new Vector3Position(-69.08f, 10.39f, 138.39f),
                QuaternionRotation = new QuaternionRotation(0, 0, 0, 0),
                NPCAttributeInfo = new NPCAttributeInfo
                {
                    HP = 10,
                    HPFull = 10,
                    Attack = 1,
                    Defence = 1,
                    ElementType = ElementTypeCode.Null,
                    ElementGauge = 0
                },
                AOISceneGrid = new AOISceneGrid(SceneCode.Island, (int)(-69.08f + 700) / 100, (int)(138.39f + 400) / 100)
            };
            OnlineNPCCache.Instance.AddNPCAISystem<AIHilichurl>(NPCCode.Hilichurl_Island_A_01, new AIHilichurl(testHilichurlGameObject));
            OnlineNPCCache.Instance.AddNPCGameObject(NPCCode.Hilichurl_Island_A_01, testHilichurlGameObject);
            OnlineNPCCache.Instance.AddOrUpdateAOINPCGameObjectDict(NPCCode.Hilichurl_Island_A_01, testHilichurlGameObject.AOISceneGrid);
        }

        private void InitEnemyFSMSystem()
        {
            FSMSystem testHilichurlFSMSystem = new FSMSystem(NPCCode.Hilichurl_Island_A_01);
            OnlineNPCCache.Instance.AddNPCFSMSystem(NPCCode.Hilichurl_Island_A_01, testHilichurlFSMSystem);
        }

        public ClientPeer FindOnlineClientPeerAsTarget(NPCCode npcCode, float patrolToChaseDis)
        {
            NPCGameObject npc = OnlineNPCCache.Instance.GetNPCGameObject(npcCode);
            List<ClientPeer> accountString = OnlineAccountCache.Instance.GetCurrentAOIClientPeerList(npc);
            for (int i = 0; i < accountString.Count; i++)
            {
                float tempDis = Vector3Position.Distance(npc.Vector3Position, accountString[i].CurrentTransformOnline.Vector3Position);
                if (tempDis < patrolToChaseDis)
                {
                    return accountString[i];
                }
            }
            return null;
        }

        public bool IsLostOnlineClientPeerTarget(NPCCode npcCode, ClientPeer targetPeer, float chaseToPatrolDis)
        {
            Vector3Position npcPos = OnlineNPCCache.Instance.GetNPCGameObject(npcCode).Vector3Position;
            float tempDis = Vector3Position.Distance(npcPos, targetPeer.CurrentTransformOnline.Vector3Position);
            if (tempDis > chaseToPatrolDis)
            {
                return true;
            }
            return false;
        }

        public bool IsApproachOnlineClientPeerTarget(NPCCode npcCode, ClientPeer targetPeer, float chaseToAttackDis)
        {
            Vector3Position npcPos = OnlineNPCCache.Instance.GetNPCGameObject(npcCode).Vector3Position;
            float tempDis = Vector3Position.Distance(npcPos, targetPeer.CurrentTransformOnline.Vector3Position);
            if (tempDis < chaseToAttackDis)
            {
                return true;
            }
            return false;
        }

        public bool IsAwayOnlienClientPeerTarget(NPCCode npcCode, ClientPeer targetPeer, float attackToChaseDis)
        {
            Vector3Position npcPos = OnlineNPCCache.Instance.GetNPCGameObject(npcCode).Vector3Position;
            float tempDis = Vector3Position.Distance(npcPos, targetPeer.CurrentTransformOnline.Vector3Position);
            if (tempDis > attackToChaseDis)
            {
                return true;
            }
            return false;
        }
    }
}
