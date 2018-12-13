using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Improbable.Gdk.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class PauseTester : MonoBehaviour
    {
        public LogView logView;

        private int numUpdates;

        private AutoResetEvent resumedEvent;

        private string filePath;

        public Button pauseButton;

        public WorkerSystem worker;

        public bool fakePause = false;

        public void OnEnable()
        {
            resumedEvent = null;

            filePath = Path.Combine(Application.persistentDataPath, "log-file.txt");

            filePath = new Regex("[\\/]").Replace(filePath, Path.DirectorySeparatorChar.ToString());

            logView.AddLog($"Log Path: {filePath}");

            logView.logPath = filePath;
            logView.ClearFile();
            logView.AddLog($"Started! (frame: {Time.frameCount} update:{numUpdates})");
        }

        public void Update()
        {
            numUpdates++;
        }

        public void TogglePause()
        {
            fakePause = !fakePause;

            logView.AddLog($"Fake Pause: {fakePause} (frame: {Time.frameCount} update:{numUpdates})");

            if (fakePause)
            {
                pauseButton.GetComponentInChildren<Text>().text = "Fake Resume";
            }
            else
            {
                pauseButton.GetComponentInChildren<Text>().text = "Fake Pause";
            }

            worker.Paused = fakePause;
        }

        public void OnApplicationPause(bool paused)
        {
            logView.AddLog($"Pause: {paused} (frame: {Time.frameCount} update:{numUpdates})");

            if (paused)
            {
                if (resumedEvent == null)
                {
                    resumedEvent = new AutoResetEvent(false);

                    logView.AddLog("Making the worker start poking now");
                    worker.PokeConnectionFromThread();

                    new Thread(() =>
                    {
                        while (true)
                        {
                            logView.AddFileLog($"Tick from thread... num updates:{numUpdates}");

                            if (resumedEvent.WaitOne(1000))
                            {
                                break;
                            }
                        }
                    }).Start();
                }
            }
            else
            {
                logView.AddLog("Making the worker stop poking now");

                worker.StopPoking();

                logView.AddLog("The worker stopped poking!");

                if (resumedEvent != null)
                {
                    resumedEvent.Set();
                    resumedEvent = null;
                }
            }
        }

        public void OnApplicationFocus(bool paused)
        {
            logView.AddLog($"Focus: {paused} (frame: {Time.frameCount} update:{numUpdates})");
        }

        private void OnApplicationQuit()
        {
            logView.AddFileLog("OnApplicationQuit!");
        }
    }
}
