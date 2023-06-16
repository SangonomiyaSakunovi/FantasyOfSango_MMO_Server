using FantasyOfSango_MMO_Server.Bases;
using FantasyOfSango_MMO_Server.Caches;
using SangoMMOCommons.Constants;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Threads
{
    public class SyncEnemyLogicThreads : BaseThreads
    {
        private Thread _thread;

        private IEnumerable<BaseAI> enemyAILogics => OnlineNPCCache.Instance.GetNPCAIs();

        public override void Run()
        {
            _thread = new Thread(Update);
            _thread.IsBackground = true;
            _thread.Start();
        }

        public override void Stop()
        {
            _thread.Abort();
        }

        public override void Update()
        {
            Thread.Sleep(ThreadsConstant.SyncEnemyLogicSleep);
            while (true)
            {
                Thread.Sleep(ThreadsConstant.SyncEnemyLogicSleep);

                foreach (var ai in enemyAILogics)
                {
                    ai.UpdateAI();
                }
            }
        }
    }
}
