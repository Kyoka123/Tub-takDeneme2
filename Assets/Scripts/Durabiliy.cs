using UnityEngine;

public class Durabiliy : MonoBehaviour
{
    public static float durability = 100f; // baþlangýç durasý
    [SerializeField] private float multiplier = 2f; // hasar çarpaný

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2")  || collision.gameObject.CompareTag("Player"))
        {
            float carpismaSiddeti = collision.relativeVelocity.magnitude - 6f;
            if (carpismaSiddeti < 0)
            {
                carpismaSiddeti = 0;
            }
            ContactPoint temasNoktasi = collision.contacts[0];
            Vector3 darbeYonu = transform.InverseTransformPoint(temasNoktasi.point);
            if (darbeYonu.z < 1)
            {
                durability -= carpismaSiddeti * multiplier;
            }
            else
            {
                durability -= carpismaSiddeti * (multiplier - 1.8f); 
            }
            
        }
        
    }

}
