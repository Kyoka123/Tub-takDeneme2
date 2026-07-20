using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Death : MonoBehaviour
{
    Vector3 Targetpoint;
    Vector3 Targetpoint2;
    public AudioSource _audioSource;
    // Trigger enter alýyoruz ve playerýn tagine göre hareket ediyoruz
    // Trigger almamýzýn sebebi eðer collision alýrsak her collisionda olurdu

    private IEnumerator OnTriggerEnter(Collider araba)
    {
        if (araba.gameObject.tag == "Bomb" || araba.gameObject.tag == "Bullet")
        {
            Destroy(araba.gameObject);
        }
        if (!(araba.gameObject.tag == "Player" || araba.gameObject.tag == "Player2"))
        { yield break; }
        _audioSource.Play();

        yield return new WaitForSeconds(2f); // Wait for 2 seconds before executing the rest of the code

        do
        {
            Targetpoint = new Vector3(Random.Range(-20, 18), 2, Random.Range(2, 40));
            Targetpoint2 = new Vector3(Random.Range(-20, 18), 2, Random.Range(2, 40));
        }while (Vector3.Distance(Targetpoint, Targetpoint2) < 3); // Ensure that the two target points are not the same





        if (araba.gameObject.tag == "Player")
        {
            araba.transform.rotation = Quaternion.Euler(0, 0, 0);
            araba.transform.position = Targetpoint;
            araba.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            araba.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        else 
        {
            araba.transform.rotation = Quaternion.Euler(0, 0, 0);
            araba.transform.position = Targetpoint2;
            araba.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            araba.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}

