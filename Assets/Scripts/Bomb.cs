using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Bomb : MonoBehaviour
{

    public float maxExplosionForce = 45f;
    public float explosionRadius = 12f;
    public LayerMask targetLayer;
    public GameObject explosionEffect;
    public GameObject explosionSound;

    void Start()
    {
        targetLayer = LayerMask.GetMask("Outline", "Outline2");
    }

    public IEnumerator Explode()
    {
        yield return new WaitForSeconds(3.5f);
        if (gameObject == null) yield break;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);

        if (colliders.Length == 0)
        {
            Explode_VFX_SFX(explosionSound, explosionEffect, gameObject);
            yield break;
        }
        else
        {

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

                    Durabiliy.durability -= finalForce * 0.6f; // Dayanýklýlýðý azalt
                    
                }
            }
            Explode_VFX_SFX(explosionSound, explosionEffect, gameObject);
        }
    }

    public void Explode_VFX_SFX(GameObject sfx, GameObject vfx, GameObject bomb)
    {
        if (gameObject == null) return;
        GameObject vfxInstance = Instantiate(vfx, transform.position, transform.rotation);
        VisualEffect vfxComponent;
        if (vfxInstance.TryGetComponent<VisualEffect>(out vfxComponent))
        {
            vfxComponent.Play();
        }
        GameObject sfxInstance = Instantiate(sfx, transform.position, transform.rotation);
        AudioSource sfxComponent;
        if (sfxInstance.TryGetComponent<AudioSource>(out sfxComponent))
        {
            sfxComponent.Play();
        }
        Destroy(bomb);
        Destroy(vfxInstance, 3f);
        Destroy(sfxInstance, 3f);
    }
}
