using UnityEngine;
using System.Collections;

public class PowerCubekiller : MonoBehaviour
{
    public GameObject spawnedC;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Player2"))
        {
            Destroy(spawnedC);
        }
    }
}
