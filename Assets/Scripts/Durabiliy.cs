using UnityEngine;

public class Durabiliy : MonoBehaviour
{
    [SerializeField] private float durability = 100f; // baþlangýç durasý
    private Rigidbody carpanrb; //carpan arabanýn hangisi olduðunu anlamak için
    //[SerializeField] private float minvalue = 8.5f; // hasar verebilmek için min hýz
    //[SerializeField] private float maxvalue = 15f; // hasar verebilecegin max hýz
    [SerializeField] private float multiplier = 2f; // hasar çarpaný

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player2")  || collision.gameObject.CompareTag("Player"))
        {
            float carpismaSiddeti = collision.relativeVelocity.magnitude;
            ContactPoint temasNoktasi = collision.contacts[0];
            Vector3 darbeYonu = transform.InverseTransformPoint(temasNoktasi.point);
            if (darbeYonu.z < 0)
            {
                Debug.Log(gameObject.name + ": Arkadan/Yandan darbe aldým! Þiddet: " + carpismaSiddeti);
                durability -= carpismaSiddeti * multiplier;
            }
            else
            {
                durability -= carpismaSiddeti * (multiplier - 1.5f); 
            }

            Debug.Log(gameObject.name + " Kalan Durability: " + durability);
        }
    }
}
