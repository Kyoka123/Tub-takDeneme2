using System.Collections;
using UnityEngine;

public class Durabiliy : MonoBehaviour
{
    public static float durability = 100f; // baþlangýç durasý
    [SerializeField] private float multiplier = 2f; // hasar çarpaný
    private bool isCarRunning = true; // Arabanýn çalýþýp çalýþmadýðýný kontrol etmek için bir deðiþken
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float flickerDuration = 1.5f; // yanýp dönme süresi
    [SerializeField] private float flickerInterval = 0.1f; // yanýp dönme hýzý

    void Start()
    {
        StartCoroutine(DurabilityCheck()); // void update kastýrmasýn diye
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2")  || collision.gameObject.CompareTag("Player"))
        {
            float carpismaSiddeti = collision.relativeVelocity.magnitude - 6f; // baðýl þiddeti alýyoruz
            if (carpismaSiddeti < 0)
            {
                carpismaSiddeti = 0;
            }
            ContactPoint temasNoktasi = collision.contacts[0]; // hangi yönden çarptýðýný bulmak için temas noktasýný alýyoruz
            Vector3 darbeYonu = transform.InverseTransformPoint(temasNoktasi.point); // ters çevirip lokal koordinat sistemine alýyoruz
            if (darbeYonu.z < 1) // eðer çarpýþma arabanýn ön tarafýnda ise
            {
                durability -= carpismaSiddeti * multiplier;
            }
            else // deðilse yani arabanýn arka veya yan tarafýnda ise
            {
                durability -= carpismaSiddeti * (multiplier - 1.8f); 
            }
            
        }
        
    }
    
    IEnumerator DurabilityCheck() // dura 0 olunca gerekeni yapmak için
    {
       
        while (isCarRunning)
        {
            if (durability <= 0)
            {
                durability = 0;
                float elapsed = 0f;
                Material mat = _meshRenderer.material;
                Color originalColor = mat.color;

                while (elapsed < flickerDuration)
                {
                    // orijinal renk mi yoksa kýrmýzý mý, sýrayla deðiþtir
                    mat.color = (mat.color == originalColor) ? Color.red : originalColor;

                    yield return new WaitForSeconds(flickerInterval);
                    elapsed += flickerInterval;
                }

                mat.color = originalColor; // sona erince orijinal renge dön
                durability = Mathf.Clamp(durability + 1f, 0f, 100f);
            }
            yield return null;
        }

            
    }
} 

