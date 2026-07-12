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

            int randomEvent = Random.Range(0, 1);

            switch (randomEvent)
            {
                case 0:
                    yield return StartCoroutine(ControlInverter());
                    break;
                case 1:
                    // Add other events here
                    break;
            }

            yield return new WaitForSeconds(15f);
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

}
