using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Handlers;
using SangoMMOCommons.Tools;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Services
{
    public class NetService : BaseService
    {
        public static NetService Instance = null;

        private Dictionary<OperationCode, BaseHandler> HandlerDict = new Dictionary<OperationCode, BaseHandler>();

        public override void InitService()
        {
            base.InitService();
            Instance = this;
            InitHandlers();
        }

        private void InitHandlers()
        {
            DefaultHandler defaultHandler = new DefaultHandler();
            HandlerDict.Add(defaultHandler.NetOpCode, defaultHandler);
            LoginHandler loginHandler = new LoginHandler();
            HandlerDict.Add(loginHandler.NetOpCode, loginHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            HandlerDict.Add(registerHandler.NetOpCode, registerHandler);
            SyncPlayerDataHandler syncPlayerDataHandler = new SyncPlayerDataHandler();
            HandlerDict.Add(syncPlayerDataHandler.NetOpCode, syncPlayerDataHandler);
            SyncPlayerTransformHandler syncPlayerPositionHandler = new SyncPlayerTransformHandler();
            HandlerDict.Add(syncPlayerPositionHandler.NetOpCode, syncPlayerPositionHandler);
            SyncPlayerAccountHandler syncPlayerAccountHandler = new SyncPlayerAccountHandler();
            HandlerDict.Add(syncPlayerAccountHandler.NetOpCode, syncPlayerAccountHandler);
            AttackCommandHandler attackCommandHandler = new AttackCommandHandler();
            HandlerDict.Add(attackCommandHandler.NetOpCode, attackCommandHandler);
            AttackDamageHandler attackDamageHandler = new AttackDamageHandler();
            HandlerDict.Add(attackDamageHandler.NetOpCode, attackDamageHandler);
            ChooseAvaterHandler chooseAvaterHandler = new ChooseAvaterHandler();
            HandlerDict.Add(chooseAvaterHandler.NetOpCode, chooseAvaterHandler);
            ItemEnhanceHandler itemEnhanceHandler = new ItemEnhanceHandler();
            HandlerDict.Add(itemEnhanceHandler.NetOpCode, itemEnhanceHandler);
            MissionUpdateHandler missionUpdateHandler = new MissionUpdateHandler();
            HandlerDict.Add(missionUpdateHandler.NetOpCode, missionUpdateHandler);
            ChatHandler chatHandler = new ChatHandler();
            HandlerDict.Add(chatHandler.NetOpCode, chatHandler);
        }

        public void OnOperationRequestDistribute(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            BaseHandler handler = DictTool.GetDictValue<OperationCode, BaseHandler>(sangoNetMessage.MessageHead.OperationCode, HandlerDict);
            if (handler != null)
            {
                handler.OnOperationRequest(sangoNetMessage, peer);
            }
            else
            {
                BaseHandler defaultHandler = DictTool.GetDictValue<OperationCode, BaseHandler>(OperationCode.Default, HandlerDict);
                defaultHandler.OnOperationRequest(sangoNetMessage, peer);
            }
        }
    }
}
