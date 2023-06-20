using System;

//Developer : SangonomiyaSakunovi

namespace SangoTimer
{
    public abstract class BaseTimer
    {
        public Action<string> LogInfoCallBack;
        public Action<string> LogWarnCallBack;
        public Action<string> LogErrorCallBack;

        protected int _taskId = 0;
        protected abstract int GenerateTaskId();

        /// <summary>
        /// Add task which need time to start
        /// </summary>
        /// <param name="delayTime">the delay time</param>
        /// <param name="taskCallBack">the add task call back</param>
        /// <param name="cancelCallBack">the cancel task call back</param>
        /// <param name="count">the times which task rework</param>
        /// <returns>the Id of this task</returns>
        public abstract int AddTask(uint delayTime, Action<int> taskCallBack, Action<int> cancelCallBack, int count = 1);

        public abstract bool DeleteTask(int taskId);

        public abstract void Reset();
    }
}
