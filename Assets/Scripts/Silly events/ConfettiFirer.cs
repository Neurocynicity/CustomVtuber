using UnityEngine;

public class ConfettiFirer : MonoBehaviour
{
    public ParticleSystem confetti;
    
    private IUserRepresentation _userRepresentation => UserRepresentationManager.Instance.CurrentUserRepresentation;

    bool shown = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (shown)
            {
                confetti.transform.position = _userRepresentation.GetPosition().position;
                confetti.Play();
                _userRepresentation.SetInvisible();
            }
            else
                _userRepresentation.SetVisible();

            shown = !shown;
        }
    }
}
