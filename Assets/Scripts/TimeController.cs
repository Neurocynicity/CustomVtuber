using UnityEngine;

[ExecuteInEditMode]
public class TimeController : MonoBehaviour
{
    [Range(0f, 2f)]
    public float timeSpeed = 1f;

    private void Update()
    {
        Time.timeScale = timeSpeed;
    }
}
