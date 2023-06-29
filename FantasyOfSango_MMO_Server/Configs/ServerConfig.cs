//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Configs
{
    public class ServerConfig
    {
        private SangoServerModeCode sangoServerMode = SangoServerModeCode.Offline;
        private SangoApplicationCode sangoApplication = SangoApplicationCode.FOS_MMO;
        private MongoDBNameCode mongoDBNameCode = MongoDBNameCode.SangoServerGameDB;
        private MongoDBAddressCode mongoDBAddressCode = MongoDBAddressCode.LocalAddress;

        public static ServerConfig Instance;

        public void InitConfig()
        {
            Instance = this;
        }

        #region ServerIPConfig
        public string GetIPAddress()
        {
            string ipAddress = "";
            switch (sangoServerMode)
            {
                case SangoServerModeCode.Offline:
                    ipAddress = "127.0.0.1";
                    break;
                case SangoServerModeCode.Online:
                    ipAddress = "124.220.20.98";
                    break;
            }
            return ipAddress;
        }

        public int GetIPPort()
        {
            int port = 0;
            switch (sangoApplication)
            {
                case SangoApplicationCode.FOS_MMO:
                    port = 52022;
                    break;
                case SangoApplicationCode.FOS_AR:
                    port = 52037;
                    break;
            }
            return port;
        }

        private enum SangoServerModeCode
        {
            Offline,
            Online
        }

        private enum SangoApplicationCode
        {
            FOS_MMO,
            FOS_AR
        }
        #endregion

        #region MongoDBConfig
        public string GetMongoDBName()
        {
            string mongoDBName = "";
            switch (mongoDBNameCode)
            {
                case MongoDBNameCode.SangoServerGameDB:
                    {
                        mongoDBName = "SangoServerGameDB";
                    }
                    break;
                case MongoDBNameCode.RemoteServerDB:
                    {
                        mongoDBName = "RemoteServerDB";
                    }
                    break;
            }
            return mongoDBName;
        }

        public string GetMongoDBAddress()
        {
            string mongoDBAddress = "";
            switch (mongoDBAddressCode)
            {
                case MongoDBAddressCode.LocalAddress:
                    {
                        mongoDBAddress = "mongodb://127.0.0.1:27017";
                    }
                    break;
                case MongoDBAddressCode.RemoteAddress:
                    {
                        mongoDBAddress = "mongodb://RemoteIP:RemotePort";
                    }
                    break;
            }
            return mongoDBAddress;
        }

        private enum MongoDBNameCode
        {
            SangoServerGameDB,
            RemoteServerDB
        }

        private enum MongoDBAddressCode
        {
            LocalAddress,
            RemoteAddress
        }
        #endregion
    }
}
