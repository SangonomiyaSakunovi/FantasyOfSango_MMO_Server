using System;
using System.Collections.Generic;

//Developer: SangonomiyaSakunovi

namespace SangoTimer
{
    public class FrameTimer : BaseTimer
    {
        private readonly Dictionary<int, FrameTask> FrameTaskDict;
        private List<int> FrameTaskList;

        private ulong currentFrame;

        private const string TaskIdLock = "FrameTimer_TaskIdLock";

        public FrameTimer(ulong currentFrameId = 0)
        {
            currentFrame = currentFrameId;
            FrameTaskDict = new Dictionary<int, FrameTask>();
            FrameTaskList = new List<int>();
        }

        public override int AddTask(uint delayTime, Action<int> taskCallBack, Action<int> cancelCallBack, int count = 1)
        {
            int taskId = GenerateTaskId();
            ulong destinationFrame = currentFrame + delayTime;
            FrameTask frameTask = new FrameTask(taskId, delayTime, count, destinationFrame, taskCallBack, cancelCallBack);
            if (FrameTaskDict.ContainsKey(taskId))
            {
                LogWarnCallBack?.Invoke($"key:{taskId} already exist in FrameTaskDict.");
                return -1;
            }
            else
            {
                FrameTaskDict.Add(taskId, frameTask);
                return taskId;
            }
        }

        public override bool DeleteTask(int taskId)
        {
            if (FrameTaskDict.TryGetValue(taskId, out FrameTask frameTask))
            {
                if (FrameTaskDict.Remove(taskId))
                {
                    frameTask.CancelCallBack?.Invoke(taskId);
                    return true;
                }
                else
                {
                    LogErrorCallBack?.Invoke($"Remove taskId:{taskId} in FrameTaskDict failed.");
                    return false;
                }
            }
            else
            {
                LogWarnCallBack?.Invoke($"TaskId:{taskId} is not exist in FrameTaskDict.");
                return false;
            }
        }

        public override void Reset()
        {
            FrameTaskDict.Clear();
            FrameTaskList.Clear();
            currentFrame = 0;
            _taskId = 0;
        }

        protected override int GenerateTaskId()
        {
            lock (TaskIdLock)
            {
                while (true)
                {
                    ++_taskId;
                    if (_taskId == int.MaxValue)
                    {
                        _taskId = 0;
                    }
                    if (!FrameTaskDict.ContainsKey(_taskId))
                    {
                        return _taskId;
                    }
                }
            }
        }

        public void UpdateTask()
        {
            currentFrame++;
            FrameTaskList.Clear();
            foreach (var item in FrameTaskDict)
            {
                FrameTask frameTask = item.Value;
                if (frameTask.DestinationFrame <= currentFrame)
                {
                    frameTask.TaskCallBack?.Invoke(frameTask.TaskId);
                    frameTask.DestinationFrame += frameTask.DelayFrame;
                    frameTask.Count--;
                    if (frameTask.Count == 0)
                    {
                        FrameTaskList.Add(frameTask.TaskId);
                    }
                }
            }
            for (int i = 0; i < FrameTaskList.Count; i++)
            {
                if (FrameTaskDict.Remove(FrameTaskList[i]))
                {
                    LogInfoCallBack?.Invoke($"Task taskId:{FrameTaskList[i]} has Run to Done.");
                }
                else
                {
                    LogErrorCallBack?.Invoke($"Remove taskId:{FrameTaskList[i]} task in FrameTaskDict failed.");
                }
            }
        }

        private class FrameTask
        {
            public int TaskId;
            public uint DelayFrame;
            public int Count;
            public ulong DestinationFrame;
            public Action<int> TaskCallBack;
            public Action<int> CancelCallBack;

            public FrameTask(int taskId, uint delayFrame, int count, ulong destinationFrame, Action<int> taskCallBack, Action<int> cancelCallBack)
            {
                TaskId = taskId;
                DelayFrame = delayFrame;
                Count = count;
                DestinationFrame = destinationFrame;
                TaskCallBack = taskCallBack;
                CancelCallBack = cancelCallBack;
            }
        }
    }
}
