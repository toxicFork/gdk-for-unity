using System;
using System.Threading;
using UnityEngine;

namespace Improbable.GDK.EditorDiscovery
{
    internal abstract class ThreadHandle
    {
        private Thread _thread;

        protected readonly ManualResetEvent KillTrigger = new ManualResetEvent(false);

        private bool _isKilled;
        private bool _isStarted;

        internal void Start()
        {
            if (_isKilled)
            {
                throw new Exception("This thread handle was killed.");
            }

            if (_isStarted)
            {
                throw new Exception("Cannot start a thread handle twice.");
            }

            _thread = new Thread(() =>
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

            _thread.Start();

            _isStarted = true;
        }

        protected abstract void ThreadMethod();

        public void Kill(bool wait = false)
        {
            if (_isKilled)
            {
                throw new Exception("This thread handle was already killed.");
            }

            KillTrigger.Set();

            if (wait)
            {
                if (!_thread.Join(1000))
                {
                    _thread.Abort();
                    Debug.LogError("Server did not die within 1 second of kill message.");
                }
            }

            _isKilled = true;
        }
    }
}
