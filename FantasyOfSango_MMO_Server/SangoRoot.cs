using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Configs;
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
        public IOCPPeer<ClientPeer> ServerInstance;

        public void InitRoot()
        {
            Instance = this;
            InitSettings();
        }

        public void RunSangoServer()
        {
            int maxConnectCount = ServerConstant.ServerMaxConnectCount;
            string iPAddress = ServerConfig.Instance.GetIPAddress();
            int port = ServerConfig.Instance.GetIPPort();
            ServerInstance = new IOCPPeer<ClientPeer>();
            ServerInstance.InitServer(iPAddress, port, maxConnectCount);            
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
            InitConfig();
            InitLog();
            InitService();
            InitCache();
            InitSystem();
        }

        private void InitConfig()
        {
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.InitConfig();
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
