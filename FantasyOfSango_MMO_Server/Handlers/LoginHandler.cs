using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Systems;
using SangoMMOCommons.Classs;
using SangoMMONetProtocol;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Handlers
{
    public class LoginHandler : BaseHandler
    {
        public LoginHandler() { NetOpCode = OperationCode.Login; }

        public override void OnOperationRequest(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            string loginReqJson = sangoNetMessage.MessageBody.MessageString;
            LoginReq loginReq = DeJsonString<LoginReq>(loginReqJson);
            string account = loginReq.Account;
            string password = loginReq.Password;
            UserInfo lookUpUserInfo = LoginSystem.Instance.LookUpUserInfo(account);
            bool isAccountPasswordMatch = false;
            bool isAccountOnline = false;
            if (lookUpUserInfo != null)
            {
                if (lookUpUserInfo.Password == password)
                {
                    isAccountPasswordMatch = true;
                    isAccountOnline = OnlineAccountCache.Instance.IsAccountOnline(account);
                }
            }
            ReturnCode returnCode = ReturnCode.Fail;

            if (isAccountPasswordMatch)
            {
                if (!isAccountOnline)
                {
                    lock (this)
                    {
                        LoginSystem.Instance.InitOnlineAccountCache(account, peer);
                    }
                    returnCode = ReturnCode.Success;
                }
                else
                {
                    returnCode = ReturnCode.AccountOnline;
                }
            }
            peer.SendOperationResponse(NetOpCode, returnCode);
        }
    }
}
