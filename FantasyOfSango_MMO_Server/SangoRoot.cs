using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Services;
using FantasyOfSango_MMO_Server.Systems;
using SangoIOCPNet;
using SangoLog;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server
{
    public class SangoRoot
    {
        public static SangoRoot Instance;
        private IOCPPeer<ClientPeer> ServerInstance;

        int maxConnectCount = ServerConstant.ServerMaxConnectCount;
        string localIPAddress = "127.0.0.1";
        int localIPPort = 52022;

        public void InitRoot()
        {
            Instance = this;
        }

        public void RunSangoServer()
        {
            ServerInstance = new IOCPPeer<ClientPeer>();
            ServerInstance.InitServer(localIPAddress, localIPPort, maxConnectCount);
            InitSettings();
            IOCPLog.Done("SangoServer is Run!");
            OnSangoServerRun();
        }

        private void OnSangoServerRun()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "Quit")
                {
                    TearDownSangoServer();
                }
            }
        }

        private void TearDownSangoServer()
        {
            ServerInstance.CloseServer();
            IOCPLog.Done("SangoServer is Tear Down");
        }

        private void InitSettings()
        {
            InitLog();
            InitService();
            InitCache();
            InitSystem();
        }

        private void InitLog()
        {
            LogConfig logConfig = new LogConfig();
            LogTool.InitSettings(logConfig);
        }

        private void InitService()
        {
            NetService netService = new NetService();
            netService.InitService();
            MongoDBService mongoDBService = new MongoDBService();
            mongoDBService.InitService();
            ResourceService resourceService = new ResourceService();
            resourceService.InitService();
            TimerService timerService = new TimerService();
            timerService.InitService();
        }

        private void InitCache()
        {
            OnlineAccountCache onlineAccountCache = new OnlineAccountCache();
            onlineAccountCache.InitCache();
            OnlineAttackCache onlineAttackCache = new OnlineAttackCache();
            onlineAttackCache.InitCache();
            OnlineNPCCache onlineNPCCache = new OnlineNPCCache();
            onlineNPCCache.InitCache();
        }

        private void InitSystem()
        {
            LoginSystem loginSystem = new LoginSystem();
            loginSystem.InitSystem();
            RegisterSystem registerSystem = new RegisterSystem();
            registerSystem.InitSystem();
            OnlineAttackSystem onlineAttackSystem = new OnlineAttackSystem();
            onlineAttackSystem.InitSystem();
            AOISystem aoiSystem = new AOISystem();
            aoiSystem.InitSystem();
            NPCSystem npcSystem = new NPCSystem();
            npcSystem.InitSystem();
            PredictSystem predictSystem = new PredictSystem();
            predictSystem.InitSystem();
            ItemEnhanceSystem itemEnhanceSystem = new ItemEnhanceSystem();
            itemEnhanceSystem.InitSystem();
            MissionUpdateSystem missionUpdateSystem = new MissionUpdateSystem();
            missionUpdateSystem.InitSystem();
            SyncPlayerTransformSystem syncPlayerTransformSystem = new SyncPlayerTransformSystem();
            syncPlayerTransformSystem.InitSystem();
        }
    }
}
