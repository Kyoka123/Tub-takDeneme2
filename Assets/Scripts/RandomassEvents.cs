using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RandomassEvents : MonoBehaviour
{
    public GameObject Car;
    public GameObject Car2;
    public Bomb bomb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EventRandomizer());
    }

    private IEnumerator EventRandomizer()
    {
        while (true)
        {

            int randomEvent = Random.Range(0, 3);

            switch (randomEvent)
            {
                case 0:
                    yield return StartCoroutine(ControlInverter());
                    break;
                case 1:
                    yield return StartCoroutine(PlaceSwapper());
                    break;
                case 2:
                    yield return StartCoroutine(BombRain());
                    break;
            }

            yield return new WaitForSeconds(10f);
        }
    }

    private IEnumerator ControlInverter()
    {
            Car.GetComponent<CarMovement>().inverter = -1;
            Car2.GetComponent<CarMovement2>().inverter = -1;
            yield return new WaitForSeconds(8f);
            Car.GetComponent<CarMovement>().inverter = 1;
            Car2.GetComponent<CarMovement2>().inverter = 1;
    }

    private IEnumerator PlaceSwapper()
    {
        yield return new WaitForSeconds(Random.Range(3, 8));
        Quaternion tempDirection = Car.transform.rotation;
        Vector3 tempPosition = Car.transform.position;
        Vector3 tempVelocity = Car.GetComponent<Rigidbody>().linearVelocity;
        Car.transform.rotation = Car2.transform.rotation;
        Car2.transform.rotation = tempDirection;
        Car.transform.position = Car2.transform.position;
        Car2.transform.position = tempPosition;
        Car.GetComponent<Rigidbody>().linearVelocity = Car2.GetComponent<Rigidbody>().linearVelocity;
        Car2.GetComponent<Rigidbody>().linearVelocity = tempVelocity;
    }

    private IEnumerator BombRain()
    {
        for (int i = 0; i <= 10; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-20, 18), 40, Random.Range(2, 40));
            GameObject bombCopy = Instantiate(bomb.gameObject, spawnPos, Random.rotation);
            StartCoroutine(bombCopy.GetComponent<Bomb>().Explode());
            yield return new WaitForSeconds(1.2f);
        }
    }

}
