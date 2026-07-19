using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject CarWillBeHit;
    public Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == CarWillBeHit)
        {
            Debug.Log("Mermi diger arabaya carpti!");

            // Buraya durabilty eksiltme kodunu yapistir coni

            StartCoroutine(WaitAndDestroy(0.01f));
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = Vector3.zero; // Mermiyi durdur
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
