using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class RegisterHandler : BaseHandler
    {
        public RegisterHandler() { NetOpCode = OperationCode.Register; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string registerReqJson = sangoNetMessage.MessageBody.MessageString;
            RegisterReq registerReq = DeJsonString<RegisterReq>(registerReqJson);
            string account = registerReq.Account;
            string password = registerReq.Password;
            string nickname = registerReq.Nickname;
            bool isAccountHasRegist = RegisterSystem.Instance.IsAccountHasRegist(account);
            bool isRegistSuccess = false;
            if (!isAccountHasRegist)
            {
                bool isAddUserInfo = RegisterSystem.Instance.InitUserInfo(account, password, nickname);
                bool isInitAvaterInfo = RegisterSystem.Instance.InitAvaterInfo(account, nickname);
                bool isInitMissionInfo = RegisterSystem.Instance.InitMissionInfo(account);
                bool isInitItemInfo = RegisterSystem.Instance.InitItemInfo(account);
                if (isAddUserInfo && isInitAvaterInfo && isInitMissionInfo && isInitItemInfo)
                {
                    isRegistSuccess = true;
                }
            }
            ReturnCode returnCode = ReturnCode.Fail;
            if (isRegistSuccess)
            {
                returnCode = ReturnCode.Success;
            }
            peer.SendOperationResponse(NetOpCode, returnCode);
        }
    }
}
