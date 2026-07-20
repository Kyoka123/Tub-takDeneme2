using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject _hitParticle;
    public GameObject CarWillBeHit;
    public Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == CarWillBeHit)
        {
            Debug.Log("Mermi diger arabaya carpti!");
            Quaternion reversedRotation = Quaternion.LookRotation(-transform.forward);
            GameObject _hitParticleClone = Instantiate(_hitParticle, transform.position - (transform.forward * 3f), reversedRotation);
            Destroy(_hitParticleClone, 0.3f);
            _rb.linearVelocity = Vector3.zero;
            Destroy(gameObject, 0.005f);
            // Buraya durabilty eksiltme kodunu yapistir coni

        }
    }
}