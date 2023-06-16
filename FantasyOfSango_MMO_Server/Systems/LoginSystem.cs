using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using FantasyOfSango_MMO_Server.Constants;
using FantasyOfSango_MMO_Server.Enums;
using FantasyOfSango_MMO_Server.Services;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class LoginSystem : BaseSystem
    {
        public static LoginSystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public UserInfo LookUpUserInfo(string account)
        {
            string collectionName = MongoDBCollectionConstant.UserInfos;
            string objectId = MongoDBIdConstant.UserInfo_ + account;
            return MongoDBService.Instance.LookUpOneData<UserInfo>(collectionName, objectId);
        }

        public void InitOnlineAccountCache(string account, ClientPeer peer)
        {
            string avaterCollectionName = MongoDBCollectionConstant.AvaterInfos;
            string avaterObjectId = MongoDBIdConstant.AvaterInfo_ + account;
            string missionCollectionName = MongoDBCollectionConstant.MissionInfos;
            string missionObjectId = MongoDBIdConstant.MissionInfo_ + account;
            string itemCollectionName = MongoDBCollectionConstant.ItemInfos;
            string itemObjectId = MongoDBIdConstant.ItemInfo_ + account;

            AvaterInfo avaterInfo = MongoDBService.Instance.LookUpOneData<AvaterInfo>(avaterCollectionName, avaterObjectId);
            MissionInfo missionInfo = MongoDBService.Instance.LookUpOneData<MissionInfo>(missionCollectionName, missionObjectId);
            ItemInfo itemInfo = MongoDBService.Instance.LookUpOneData<ItemInfo>(itemCollectionName, itemObjectId);

            peer.SetAccount(account);
            peer.SetAvaterInfo(avaterInfo);
            peer.SetMissionInfo(missionInfo);
            peer.SetItemInfo(itemInfo);
            peer.SetCurrentAvaterIndexByAvaterCode(AvaterCode.SangonomiyaKokomi);
            peer.SetPeerEnhanceModeCode(PeerEnhanceModeCode.Done);

            OnlineAccountCache.Instance.AddOnlineAccount(peer, account);
        }
    }
}
