using UnityEngine;

public class UserRepresentationManager : MonoBehaviour
{
    public static UserRepresentationManager Instance;

    private void Awake()
    {
        Instance = this;
        CurrentUserRepresentation = _userRepresentations[_currentRepresentationIndex].UserRepresentation;
    }

    [SerializeField] private UserRepresentationHolder[] _userRepresentations;
    [SerializeField] private KeyCode[] _swapKey;

    public IUserRepresentation CurrentUserRepresentation;
    private int _currentRepresentationIndex;


    private KeybindManager.Keybind _swapKeybind;
    private void OnEnable()
    {
        _swapKeybind = new KeybindManager.Keybind(
            UpdateUserRepresentation,
            _swapKey
            );
        
        KeybindManager.Instance.AddKeybind(_swapKeybind);
    }

    private void OnDisable()
    {
        KeybindManager.Instance.RemoveKeybind(_swapKeybind);
    }

    private void UpdateUserRepresentation()
    {
        CurrentUserRepresentation.SetInvisible();

        _currentRepresentationIndex++;
        _currentRepresentationIndex %= _userRepresentations.Length;
        
        CurrentUserRepresentation = _userRepresentations[_currentRepresentationIndex].UserRepresentation;
        CurrentUserRepresentation.SetVisible();
    }
}
