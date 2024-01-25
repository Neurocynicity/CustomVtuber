using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleInstanceDetector : MonoBehaviour
{
    const string _PROCESS_NAME = "MyCustomVtuber";

    [SerializeField] private bool _debugProcessNames;

    private void Awake()
    {
        if (!IsAlreadyRunning())
            OpenVtuber();
    }

    public void OpenVtuber() =>
        SceneManager.LoadScene("Vtoobing");

    public void OpenConnectionScene() =>
        SceneManager.LoadScene("Connector");

    bool IsAlreadyRunning()
    {
        System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
        foreach (System.Diagnostics.Process process in processes)
        {
            if (_debugProcessNames)
                Debug.Log($"Found Process: {process.ProcessName}");
            
            if (process.ProcessName == _PROCESS_NAME)
                return true;
        }

        return false;
    }
}
