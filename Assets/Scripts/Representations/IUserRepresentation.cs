using UnityEngine;

public interface IUserRepresentation
{
    public Transform GetPosition();
    
    public void EnablePhysics();
    public void DisablePhysics();

    public void SetInvisible();
    public void SetVisible();

    public void ResetRepresentation();

    public void AddExplosionForce(float force);
}