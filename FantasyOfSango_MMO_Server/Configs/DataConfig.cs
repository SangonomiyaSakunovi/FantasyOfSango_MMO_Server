//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Configs
{
    public class DataConfig<T>
    {
        public string _id;
    }

    public class MissionConfig : DataConfig<MissionConfig>
    {
        public string npcID;
        public string actionID;
        public int coinRewards;
        public int worldExpRewards;
        public string material1Rewards;
        public string material2Rewards;
    }

    public class WeaponBreakConfig : DataConfig<WeaponBreakConfig>
    {
        public int weaponBreakCoin;
        public string weaponBreakMaterial1;
        public string weaponBreakMaterial2;
    }

    public class WeaponDetailsConfig : DataConfig<WeaponDetailsConfig>
    {
        public int weaponQuanlity;
    }

    public class WeaponValueConfig : DataConfig<WeaponValueConfig>
    {
        public int weaponBaseATK;
        public string weaponAbility1;
        public string weaponAbility2;
        public int weaponEnhanceLevelExp;
        public int weaponAccumulateExp;
    }
}
