using TMPro;
using UnityEngine;

public class TMPCycleTextColour : MonoBehaviour
{
    [SerializeField] private float _colourChangeSpeed = 1f;
    [SerializeField] private TMP_Text _text;
    void Update()
    {
        Color colourThisFrame = _text.color;
        Color.RGBToHSV(colourThisFrame, out float h, out float s, out float v);
        h += _colourChangeSpeed * Time.deltaTime;
        h %= 1;
        
        _text.color = Color.HSVToRGB(h, s, v);
    }
}
