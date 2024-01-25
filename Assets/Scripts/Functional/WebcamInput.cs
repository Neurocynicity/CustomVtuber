using UnityEngine;
using UnityEngine.UI;

public class WebcamInput : MonoBehaviour
{
    [SerializeField] private RawImage _rawimage;
    
    void Start ()
    {
        WebCamTexture webcamTexture = new WebCamTexture();
        _rawimage.texture = webcamTexture;
        _rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
}
