using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMOCommons.Constants;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class SyncEnemyLogicSystem
    {
        private IEnumerable<BaseAI> enemyAILogics => OnlineNPCCache.Instance.GetNPCAIs();

        public void Update()
        {
            while (true)
            {
                foreach (var ai in enemyAILogics)
                {
                    ai.UpdateAI();
                }
            }
        }
    }
}
