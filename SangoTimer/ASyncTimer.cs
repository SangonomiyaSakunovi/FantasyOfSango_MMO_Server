using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

//Developer: SangonomiyaSakunovi

namespace SangoTimer
{
    public class ASyncTimer : BaseTimer
    {
        private readonly ConcurrentDictionary<int, ASyncTask> ASyncTaskDict;
        private readonly ConcurrentQueue<ASyncTaskPack> ASyncTaskPackQueue;
        private readonly bool isSetHandleThread;

        private const string TaskIdLock = "ASyncTimer_TaskIdLock";

        public ASyncTimer(bool isSetHandleThreads)
        {
            isSetHandleThread = isSetHandleThreads;
            ASyncTaskDict = new ConcurrentDictionary<int, ASyncTask>();
            if (isSetHandleThread)
            {
                ASyncTaskPackQueue = new ConcurrentQueue<ASyncTaskPack>();
            }
        }

        public override int AddTask(uint delayTime, Action<int> taskCallBack, Action<int> cancelCallBack, int count = 1)
        {
            int taskId = GenerateTaskId();
            ASyncTask asyncTask = new ASyncTask(taskId, delayTime, count, taskCallBack, cancelCallBack);
            RunTaskInThreadPool(asyncTask);

            if (ASyncTaskDict.TryAdd(taskId, asyncTask))
            {
                return taskId;
            }
            else
            {
                LogWarnCallBack?.Invoke($"key:{taskId} already exist in ASyncTaskDict.");
                return -1;
            }
        }

        public override bool DeleteTask(int taskId)
        {
            if (ASyncTaskDict.TryRemove(taskId,out ASyncTask asyncTask))
            {
                LogInfoCallBack?.Invoke($"Remove taskId: {asyncTask.TaskId} task in ASyncTaskDict success.");
                asyncTask.CancellationTokenSource.Cancel();
                if (isSetHandleThread && asyncTask.CancelCallBack != null)
                {
                    ASyncTaskPackQueue.Enqueue(new ASyncTaskPack(asyncTask.TaskId, asyncTask.CancelCallBack));
                }
                else
                {
                    asyncTask.CancelCallBack?.Invoke(asyncTask.TaskId);
                }
                return true;
            }
            else
            {
                LogErrorCallBack?.Invoke($"Remove taskId: {asyncTask.TaskId} ASyncTask in ASyncTaskDict failed.");
                return false;
            }
        }

        public override void Reset()
        {
            if (ASyncTaskPackQueue !=null && !ASyncTaskPackQueue.IsEmpty)
            {
                LogWarnCallBack?.Invoke("ASyncTaskPackQueue is not empty.");
            }
            ASyncTaskDict.Clear();
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
                    if (!ASyncTaskDict.ContainsKey(_taskId))
                    {
                        return _taskId;
                    }
                }
            }
        }

        public void HandleTask()
        {
            while(ASyncTaskPackQueue != null && ASyncTaskPackQueue.Count > 0)
            {
                if (ASyncTaskPackQueue.TryDequeue(out ASyncTaskPack asyncTaskPack))
                {
                    asyncTaskPack.ProxyCallBack(asyncTaskPack.TaskId);
                }
                else
                {
                    LogErrorCallBack?.Invoke("ASyncTaskPackQueue Dequeue Data Error.");
                }
            }
        }

        private void RunTaskInThreadPool(ASyncTask asyncTask)
        {
            Task.Run(async () =>
            {
                if (asyncTask.Count > 0)
                {
                    do
                    {
                        asyncTask.Count--;
                        asyncTask.LoopIndex++;
                        int delayTime = (int)(asyncTask.DelayTime + asyncTask.FixDelta);
                        if (delayTime > 0)
                        {
                            await Task.Delay(delayTime, asyncTask.CancellationToken);
                        }
                        TimeSpan timeSpan = DateTime.UtcNow - asyncTask.StartTime;
                        asyncTask.FixDelta = (int)(asyncTask.DelayTime * asyncTask.LoopIndex - timeSpan.TotalMilliseconds);
                        CallTaskCallBack(asyncTask);
                    } while (asyncTask.Count > 0);
                }
                else
                {
                    while (true)
                    {
                        asyncTask.LoopIndex++;
                        int delayTime = (int)(asyncTask.DelayTime + asyncTask.FixDelta);
                        if (delayTime > 0)
                        {
                            await Task.Delay(delayTime, asyncTask.CancellationToken);
                        }
                        TimeSpan timeSpan = DateTime.UtcNow - asyncTask.StartTime;
                        asyncTask.FixDelta = (int)(asyncTask.DelayTime * asyncTask.LoopIndex - timeSpan.TotalMilliseconds);
                        CallTaskCallBack(asyncTask);
                    }
                }
            });
        }

        private void CallTaskCallBack(ASyncTask asyncTask)
        {
            if (isSetHandleThread && asyncTask.TaskCallBack != null)
            {
                ASyncTaskPackQueue.Enqueue(new ASyncTaskPack(asyncTask.TaskId, asyncTask.TaskCallBack));
            }
            else
            {
                asyncTask.TaskCallBack?.Invoke(asyncTask.TaskId);
            }
            if (asyncTask.Count == 0)
            {
                if (ASyncTaskDict.TryRemove(asyncTask.TaskId,out ASyncTask tempTask))
                {
                    LogInfoCallBack?.Invoke($"TaskId: {asyncTask.TaskId} has Run to Done.");
                }
                else
                {
                    LogErrorCallBack?.Invoke($"Remove TaskId: {asyncTask.TaskId} ASyncTask in taskDict failed.");
                }
            }
        }

        private class ASyncTask
        {
            public int TaskId;
            public uint DelayTime;
            public int Count;
            public DateTime StartTime;
            public Action<int> TaskCallBack;
            public Action<int> CancelCallBack;
            public ulong LoopIndex;
            public int FixDelta;

            public CancellationTokenSource CancellationTokenSource;
            public CancellationToken CancellationToken;

            public ASyncTask(int taskId, uint delayTime, int count, Action<int> taskCallBack, Action<int> cancelCallBack)
            {
                TaskId = taskId;
                DelayTime = delayTime;
                Count = count;
                TaskCallBack = taskCallBack;
                CancelCallBack = cancelCallBack;
                StartTime = DateTime.UtcNow;
                LoopIndex = 0;
                FixDelta = 0;

                CancellationTokenSource = new CancellationTokenSource();
                CancellationToken = CancellationTokenSource.Token;
            }
        }

        private class ASyncTaskPack
        {
            public int TaskId;
            public Action<int> ProxyCallBack;

            public ASyncTaskPack(int taskId, Action<int> proxyCallBack)
            {
                TaskId = taskId;
                ProxyCallBack = proxyCallBack;
            }
        }
    }
}
