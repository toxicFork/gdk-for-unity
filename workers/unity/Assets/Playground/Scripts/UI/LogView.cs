using System;
using System.IO;
using UnityEngine;

public class LogView : MonoBehaviour
{
    public LogEntry logEntryPrefab;
    public RectTransform content;
    public string logPath;

    public void AddLog(string text)
    {
        var newLogEntry = Instantiate(logEntryPrefab, content);

        var textWithTimeStamp = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.ffff}: {text}";

        newLogEntry.textField.text = textWithTimeStamp;

        AddFileLog(text);
    }

    public void AddFileLog(string text)
    {
        var textWithTimeStamp = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.ffff}: {text}";

        Debug.Log(textWithTimeStamp);

        if (!string.IsNullOrEmpty(logPath))
        {
            File.AppendAllText(logPath, textWithTimeStamp + "\n");
        }
    }

    public void ClearFile()
    {
        if (!string.IsNullOrEmpty(logPath))
        {
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }
        }
    }
}
