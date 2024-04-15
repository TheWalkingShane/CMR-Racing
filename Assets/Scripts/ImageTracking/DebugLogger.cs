using UnityEngine;
using TMPro; // Make sure to include the TextMeshPro namespace

public class DebugLogger : MonoBehaviour
{
    public TextMeshProUGUI debugTextUI; // Assign your TextMeshProUGUI component in the inspector
    private string log;

    void OnEnable()
    {
        // Register to receive Unity log messages
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Deregister the log callback when this object is disabled/destroyed
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Append the log string to the existing text
        log = logString;
        debugTextUI.text = log + "\n" + debugTextUI.text;

        // Optionally, you can limit the number of log messages displayed
        if (debugTextUI.text.Length > 5000)
        {
            debugTextUI.text = debugTextUI.text.Substring(0, 5000);
        }
    }
}