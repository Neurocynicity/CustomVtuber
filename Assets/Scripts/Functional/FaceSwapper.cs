using System.Collections;
using UnityEngine;

public class FaceSwapper : MonoBehaviour
{
    [SerializeField] private GameObject[] _faces;
    [SerializeField] private GameObject _swappFace;
    [SerializeField] private float _swapTime;
    [SerializeField] private KeyCode[] _swapKey;

    private int _currentFaceIndex;

    private Coroutine SwapFaceCoroutine;
    private void Update()
    {
        if (!Helper.IsHotkeyBeingInputted(_swapKey))
            return;
        
        _faces[_currentFaceIndex].SetActive(false);

        _currentFaceIndex++;
        _currentFaceIndex %= _faces.Length;
        
        _swappFace.SetActive(true);
        
        if (SwapFaceCoroutine != null)
            StopCoroutine(SwapFaceCoroutine);

        SwapFaceCoroutine = StartCoroutine(SwapFace());
    }


    private IEnumerator SwapFace()
    {
        yield return new WaitForSeconds(_swapTime);
        
        _swappFace.SetActive(false);
        _faces[_currentFaceIndex].SetActive(true);
    }
}
