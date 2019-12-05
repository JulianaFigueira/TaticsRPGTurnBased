using System.Collections;
using UnityEngine;

public class LogBattle : MonoBehaviour
{
    string myLog;
    Queue myLogQueue = new Queue();
    public UnityEngine.UI.Text messageBoard;

    void Start()
    {
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            myLog = logString;
            string newString = "\n" + myLog;
            if (myLogQueue.Count >= 10)
                myLogQueue.Dequeue();
            myLogQueue.Enqueue(newString);
            myLog = string.Empty;
            foreach (string mylog in myLogQueue)
            {
                myLog += mylog;
            }


            messageBoard.text = myLog;
        }
    }
}
