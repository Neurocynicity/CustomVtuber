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
    private void Update()
    {
        if (!Helper.IsHotkeyBeingInputted(_swapKey))
            return;
        
        CurrentUserRepresentation.SetInvisible();

        _currentRepresentationIndex++;
        _currentRepresentationIndex %= _userRepresentations.Length;
        
        CurrentUserRepresentation = _userRepresentations[_currentRepresentationIndex].UserRepresentation;
        CurrentUserRepresentation.SetVisible();
    }
}
