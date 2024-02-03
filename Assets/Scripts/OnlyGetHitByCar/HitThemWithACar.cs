using System.Collections;
using UnityEngine;

public class HitThemWithACar : MonoBehaviour
{
    public Vector3 startPosition, endPosition;
    public float speed = 0.2f;

    public GameObject car;
    public float intensity = 5;

    private bool crashed;

    [SerializeField]
    private CanBeHitByCar _toHitWithCar;

    [SerializeField]
    private Transform _carForcePosition;

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
        _toHitWithCar.EnablePhysics();
        _toHitWithCar.AddExplosionForce(intensity, _carForcePosition.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (crashed)
            {
                StopAllCoroutines();
                _toHitWithCar.DisablePhysics();
                _toHitWithCar.ResetBody();
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
