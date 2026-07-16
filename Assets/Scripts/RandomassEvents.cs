using System.Collections;
using UnityEngine;

public class RandomassEvents : MonoBehaviour
{
    public CarMovement CarMovement;
    public CarMovement2 CarMovement2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EventRandomizer());
    }

    private IEnumerator EventRandomizer()
    {
        while (true)
        {

            int randomEvent = Random.Range(0, 2);

            switch (randomEvent)
            {
                case 0:
                    yield return StartCoroutine(ControlInverter());
                    break;
                case 1:
                    yield return StartCoroutine(PlaceSwapper());
                    break;
            }

            yield return new WaitForSeconds(10f);
        }
    }

    private IEnumerator ControlInverter()
    {
            CarMovement.inverter = -1;
            CarMovement2.inverter = -1;
            yield return new WaitForSeconds(8f);
            CarMovement.inverter = 1;
            CarMovement2.inverter = 1;
    }

    private IEnumerator PlaceSwapper()
    {
        yield return new WaitForSeconds(Random.Range(3, 8));
        Quaternion tempDirection = CarMovement.transform.rotation;
        Vector3 tempPosition = CarMovement.transform.position;
        Vector3 tempVelocity = CarMovement._rb.linearVelocity;
        CarMovement.transform.rotation = CarMovement2.transform.rotation;
        CarMovement2.transform.rotation = tempDirection;
        CarMovement.transform.position = CarMovement2.transform.position;
        CarMovement2.transform.position = tempPosition;
        CarMovement._rb.linearVelocity = CarMovement2._rb.linearVelocity;
        CarMovement2._rb.linearVelocity = tempVelocity;
    }

}
