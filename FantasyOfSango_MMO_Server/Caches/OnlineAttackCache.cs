using FantasyOfSango_MMO_Server.Bases;
using SangoMMOCommons.Classs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Caches
{
    public class OnlineAttackCache : BaseCache
    {
        public static OnlineAttackCache Instance = null;

        private List<AttackDamage> OnlineAttackDamageCacheList = new List<AttackDamage>();

        public override void InitCache()
        {
            base.InitCache();
            Instance = this;
        }

        public void AddOnlineAttackDamageCache(AttackDamage attackDamageCache)
        {
            OnlineAttackDamageCacheList.Add(attackDamageCache);
        }
    }
}
