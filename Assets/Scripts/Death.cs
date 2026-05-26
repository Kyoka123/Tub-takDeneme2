using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Death : MonoBehaviour
{
    Vector3 Targetpoint;
    Vector3 Targetpoint2;
    // Trigger enter alýyoruz ve playerýn tagine göre hareket ediyoruz
    // Trigger almamýzýn sebebi eðer collision alýrsak her collisionda olurdu
    private void OnTriggerEnter(Collider araba)
    {
        do
        {
           Targetpoint = new Vector3(Random.Range(-20, 18), 2, Random.Range(2, 40));
           Targetpoint2 = new Vector3(Random.Range(-20, 18), 2, Random.Range(2, 40));
        } while (Vector3.Distance(Targetpoint, Targetpoint2) < 3); // Ensure that the two target points are not the same





        if (araba.gameObject.tag == "Player")
        {
           GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.Euler(0,0,0);
           GameObject.FindGameObjectWithTag("Player").transform.position = Targetpoint;
           GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
           GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (araba.gameObject.tag == "Player2")
        {
           GameObject.FindGameObjectWithTag("Player2").transform.rotation = Quaternion.Euler(0, 0, 0);
           GameObject.FindGameObjectWithTag("Player2").transform.position = Targetpoint2;
           GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
           GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}

