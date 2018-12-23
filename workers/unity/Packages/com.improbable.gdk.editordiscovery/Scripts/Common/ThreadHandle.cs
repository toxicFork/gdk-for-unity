using System;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    public abstract class ThreadHandle
    {
        private Thread thread;

        protected readonly ManualResetEvent KillTrigger = new ManualResetEvent(false);

        private bool isKilled;
        private bool isStarted;

        public void Start()
        {
            if (isKilled)
            {
                throw new Exception("This thread handle was killed.");
            }

            if (isStarted)
            {
                throw new Exception("Cannot start a thread handle twice.");
            }

            thread = new Thread(() =>
            {
                try
                {
                    ThreadMethod();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception caught for {GetType().FullName}");
                    Debug.LogException(e);
                }
            });

            thread.Start();

            isStarted = true;
        }

        protected abstract void ThreadMethod();

        public void Kill(bool wait = false)
        {
            if (isKilled)
            {
                throw new Exception("This thread handle was already killed.");
            }

            KillTrigger.Set();

            if (wait)
            {
                if (!thread.Join(1000))
                {
                    thread.Abort();
                    Debug.LogError("Server did not die within 1 second of kill message.");
                }
            }

            isKilled = true;
        }
    }
}
