using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bomb : MonoBehaviour
{

    public float maxExplosionForce = 25f;
    public float explosionRadius = 10f;
    public LayerMask targetLayer;
    public bool hasExploded = false;

    void Start()
    {
        targetLayer = LayerMask.GetMask("Outline", "Outline2");
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.bKey.isPressed)
        {
            StartCoroutine(Explode());
        }
    }

    public IEnumerator Explode()
    {
        if (hasExploded)
        {
            yield break;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.attachedRigidbody;

            if (rb != null)
            {
                float distance = Vector3.Distance(transform.position, hit.bounds.center);
                float forceIntensity = Mathf.Clamp01(1.3f - (distance / explosionRadius));

                Vector3 direction = hit.bounds.center - transform.position;
                direction.y = 0; // Sadece X ve Z ekseni

                if (direction.magnitude > 0.01f)
                {
                    direction.Normalize();
                }
                else
                {
                    // Tam altindaysa rastgele bir yone firlat
                    direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                }

                float finalForce = forceIntensity * maxExplosionForce;

                rb.AddForce(direction * finalForce, ForceMode.Impulse);
                hasExploded = true;
            }
        }
        yield return new WaitForSeconds(1f); // 1 saniye bekle
        //Destroy(gameObject);
    }
}
