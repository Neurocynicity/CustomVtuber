using UnityEngine;

public class UserRepresentationHolder : MonoBehaviour
{
    [SerializeField, Interface(typeof(IUserRepresentation))]
    private Object _userRepresentationSerialised;
    
    public IUserRepresentation UserRepresentation => _userRepresentationSerialised as IUserRepresentation;
}
