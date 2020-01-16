using System;
using System.Collections;
using System.Threading;

namespace GambitMonkey.Threading
{
    public class ThreadWorker
    {
        private Thread ChildThread = null;
        private EventWaitHandle SuspendHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
        private EventWaitHandle AbortHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        private bool WantAbort = false;

        public ThreadWorker(IEnumerator threadImplementation) : this()
        {
            Start(threadImplementation);
        }

        public void Start(IEnumerator threadImplementation)
        {
            ChildThread = new Thread(ThreadLoop);
            ChildThread.Start(threadImplementation);
        }

        public void Resume()
        {
            SuspendHandle.Set();
        }

        public void Suspend()
        {
            if (!WantAbort)
                SuspendHandle.Reset();
        }

        public void Abort(bool block = true)
        {
            WantAbort = true;

            Resume();

            if (block)
                AbortHandle.WaitOne();
        }

        private void ThreadLoop(object threadImplementation)
        {
            try
            {
                var impl = threadImplementation as IEnumerator;

                while (!WantAbort && impl.MoveNext())
                {
                    if (WantAbort)
                        break;

                    SuspendHandle.WaitOne();
                }
            }
            catch (Exception e)
            {
                WantAbort = true;
                UnityEngine.Debug.LogException(e);
            }

            AbortHandle.Set();
            ChildThread = null;
        }

        public bool IsRunning() { return ChildThread != null; }
        public bool IsCompleted() { return ChildThread == null; }

        /// <summary>
        /// http://www.richardmeredith.net/2017/11/pause-resume-abort-multithreading-unity/5/
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="maxTime"></param>
        /// <returns></returns>
        IEnumerator TimedEnumerator(IEnumerator enumerator, float maxTime)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            stopwatch.Start();

            while (enumerator.MoveNext())
            {
                if (stopwatch.ElapsedMilliseconds > maxTime)
                {
                    yield return null;
                    stopwatch.Reset();
                    stopwatch.Start();
                }
            }
        }

#if UNITY_EDITOR
        public ThreadWorker()
        {
            UnityEditor.EditorApplication.playmodeStateChanged += HandleEditorPlayModeChanged;
        }

        ~ThreadWorker()
        {
            UnityEditor.EditorApplication.playmodeStateChanged -= HandleEditorPlayModeChanged;
        }

        void HandleEditorPlayModeChanged()
        {
            if (IsRunning())
            {
                if (UnityEditor.EditorApplication.isPaused)
                    Suspend();
                else
                    Resume();
            }
        }
#else
	public ThreadWorker() {}
#endif
    }
}