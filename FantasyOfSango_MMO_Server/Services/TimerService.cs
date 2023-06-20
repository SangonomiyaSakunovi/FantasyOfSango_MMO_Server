using FantasyOfSango_MMO_Server.Bases;
using SangoTimer;

//Developer: SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Services
{
    public class TimerService : BaseService
    {
        public static TimerService Instance = null;

        private TickTimer tickTimer;

        public override void InitService()
        {
            base.InitService();
            Instance = this;
            InitTimers();
            UpdateTickTask();
            HandleTickTask();
        }

        private void InitTimers()
        {
            tickTimer = new TickTimer(0, false);
            tickTimer.LogInfoCallBack = this.LogInfo;
            tickTimer.LogErrorCallBack = this.LogError;
            tickTimer.LogWarnCallBack = this.LogWarn;
        }

        public int AddTickTask(uint delayTime, Action<int> taskCallBack, Action<int> cancelCallBack = null, int count = 1)
        {
            return tickTimer.AddTask(delayTime, taskCallBack, cancelCallBack, count);
        }

        public bool DeletTickTask(int taskId)
        {
            return tickTimer.DeleteTask(taskId);
        }

        private void UpdateTickTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    tickTimer.UpdateTask();
                    await Task.Delay(2);
                }
            });           
        }

        private void HandleTickTask()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    tickTimer.HandleTask();
                    await Task.Delay(2);
                }
            });
        }
    }
}
