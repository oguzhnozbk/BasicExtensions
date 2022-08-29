using System.Collections.Generic;
using System.Threading;

namespace BasicExtensions
{
    public class ThreadExtensions
    {
        public int ThreadCount { get; set; }
        private Thread[] _threads = null;
        public ThreadExtensions(int threadCount)
        {
            ThreadCount = threadCount;
            CreateThread();
        }
        public void WaitDone()
        {
            while (true)
                if (AreAllThreadDone())
                    return;
        }
        public void Run(ThreadStart start)
        {
            var index = FindNullReferanceThreadIndex();
            _threads[index] = new Thread(start);
            _threads[index].Start();
        }
        private void CreateThread()
        {
            if (_threads == null)
            {
                _threads = new Thread[ThreadCount];
                for (int i = 0; i < ThreadCount; i++)
                    _threads[i] = null;
            }
            else if (_threads.Length > ThreadCount)
            {
                var thread = new Thread[ThreadCount];
                for (int i = 0; i < ThreadCount; i++)
                    thread[i] = _threads[i];
                _threads = thread;
            }
            else if (_threads.Length < ThreadCount)
            {
                var thread = new Thread[ThreadCount];
                for (int i = 0; i < _threads.Length; i++)
                    thread[i] = _threads[i];
                for (int i = _threads.Length; i < ThreadCount; i++)
                    thread[i] = _threads[i];
                _threads = thread;
            }
        }
        private int FindNullReferanceThreadIndex()
        {
            while (true)
            {
                for (var i = 0; i < ThreadCount; i++)
                {
                    if (_threads[i] == null) return i;
                    if (!_threads[i].IsAlive)
                    {
                        _threads[i].Join();
                        _threads[i] = null;
                        return i;
                    }
                }
            }
        }
        private bool AreAllThreadDone()
        {
            var control = true;
            for (var i = 0; i < ThreadCount; i++)
                if (_threads[i] != null)
                {
                    if (_threads[i].IsAlive)
                        control = false;
                    else
                        _threads[i] = null;
                }
            return control;
        }
    }
}
