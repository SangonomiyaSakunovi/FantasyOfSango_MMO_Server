using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Constants;
using FantasyOfSango_MMO_Server.Services;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Enums;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class RegisterSystem : BaseSystem
    {
        public static RegisterSystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public bool IsAccountHasRegist(string account)
        {
            string collectionName = MongoDBCollectionConstant.UserInfos;
            string objectId = MongoDBIdConstant.UserInfo_ + account;
            UserInfo lookUpUserInfo = MongoDBService.Instance.LookUpOneData<UserInfo>(collectionName, objectId);
            if (lookUpUserInfo != null)
            {
                return true;
            }
            return false;
        }

        public bool InitUserInfo(string account, string password, string nickname)
        {
            string collectionName = MongoDBCollectionConstant.UserInfos;
            UserInfo userInfo = new UserInfo
            {
                _id = MongoDBIdConstant.UserInfo_ + account,
                Account = account,
                Password = password,
                Nickname = nickname
            };
            return MongoDBService.Instance.AddOneData<UserInfo>(userInfo, collectionName);
        }

        public bool InitMissionInfo(string account)
        {
            string collectionName = MongoDBCollectionConstant.MissionInfos;
            MissionInfo missionInfo = new MissionInfo
            {
                _id = MongoDBIdConstant.MissionInfo_ + account,
                Account = account,
                MainMissionIdList = new List<string>() { "Island_Mission_Main_01_01" },
                DailyMissionIdList = new List<string>(),
                OptionalMissionIdList = new List<string>()
            };
            return MongoDBService.Instance.AddOneData<MissionInfo>(missionInfo, collectionName);
        }

        public bool InitAvaterInfo(string account, string nickname)
        {
            string collectionName = MongoDBCollectionConstant.AvaterInfos;
            AvaterAttributeInfo kokomiInfo = new AvaterAttributeInfo
            {
                Avater = AvaterCode.SangonomiyaKokomi,
                HP = 80,
                HPFull = 100,
                MP = 40,
                MPFull = 50,
                Attack = 1,
                Defence = 0,
                ElementType = ElementTypeCode.Hydro,
                ElementGauge = 2,
                WeaponId = "Sword_Test_1",
                WeaponExp = 0
            };
            AvaterAttributeInfo yoimiyaInfo = new AvaterAttributeInfo
            {
                Avater = AvaterCode.Yoimiya,
                HP = 80,
                HPFull = 100,
                MP = 40,
                MPFull = 50,
                Attack = 1,
                Defence = 0,
                ElementType = ElementTypeCode.Hydro,
                ElementGauge = 2,
                WeaponId = "Sword_Test_1",
                WeaponExp = 0
            };
            AvaterAttributeInfo ayakaInfo = new AvaterAttributeInfo
            {
                Avater = AvaterCode.Ayaka,
                HP = 80,
                HPFull = 100,
                MP = 40,
                MPFull = 50,
                Attack = 1,
                Defence = 0,
                ElementType = ElementTypeCode.Hydro,
                ElementGauge = 2,
                WeaponId = "Sword_Test_1",
                WeaponExp = 0
            };
            AvaterAttributeInfo aetherInfo = new AvaterAttributeInfo
            {
                Avater = AvaterCode.Aether,
                HP = 80,
                HPFull = 100,
                MP = 40,
                MPFull = 50,
                Attack = 1,
                Defence = 0,
                ElementType = ElementTypeCode.Hydro,
                ElementGauge = 2,
                WeaponId = "Sword_Test_1",
                WeaponExp = 0
            };

            AvaterInfo avaterInfo = new AvaterInfo
            {
                _id = MongoDBIdConstant.AvaterInfo_ + account,
                Account = account,
                Nickname = nickname,
                AttributeInfoList = new List<AvaterAttributeInfo>() { kokomiInfo, yoimiyaInfo, ayakaInfo, aetherInfo }
            };
            return MongoDBService.Instance.AddOneData<AvaterInfo>(avaterInfo, collectionName);
        }

        public bool InitItemInfo(string account)
        {
            string collectionName = MongoDBCollectionConstant.ItemInfos;
            ItemInfo itemInfo = new ItemInfo
            {
                _id = MongoDBIdConstant.ItemInfo_ + account,
                Account = account,
                Coin = 500,
                WeaponEnhanceMaterialList = new List<string>()
            };
            return MongoDBService.Instance.AddOneData<ItemInfo>(itemInfo, collectionName);
        }
    }
}
