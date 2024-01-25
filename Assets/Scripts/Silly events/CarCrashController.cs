using System.Collections;
using UnityEngine;

public class CarCrashController : MonoBehaviour
{
    public Vector3 startPosition, endPosition;
    public float speed = 0.2f;

    public GameObject car;
    public float intensity = 5;

    private bool crashed;
    
    private IUserRepresentation UserRepresentation => UserRepresentationManager.Instance.CurrentUserRepresentation;

    private IEnumerator CarCrash()
    {
        float startTime = Time.time;
        float endTime = startTime + speed;

        car.SetActive(true);
        
        while (Time.time < endTime)
        {
            float normalisedTime = (Time.time - startTime) / speed;

            car.transform.position = Vector3.Lerp(startPosition, endPosition, normalisedTime);
            yield return null;
        }

        car.transform.position = endPosition;

        // Here I add an explosive force to the cube so we aren't doing silly unncesecery collisions
        UserRepresentation.EnablePhysics();
        UserRepresentation.AddExplosionForce(intensity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (crashed)
            {
                StopAllCoroutines();
                UserRepresentation.DisablePhysics();
                UserRepresentation.ResetRepresentation();
                car.SetActive(false);
            }
            else
            {
                StartCoroutine(CarCrash());
            }
            crashed = !crashed;
        }
    }
}
