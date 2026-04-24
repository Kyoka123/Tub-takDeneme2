using UnityEngine;

public class Death : MonoBehaviour
{
    Vector3 Targetpoint;
   
    
    private void OnTriggerEnter(Collider other)
    {
        Targetpoint = new Vector3(Random.Range(0, 10), 2, Random.Range(5, 15));
        if (other.gameObject.tag == "Player")
        {
           GameObject.FindGameObjectWithTag("Player").transform.position = Targetpoint;
        }
        if (other.gameObject.tag == "Player2")
        {
            GameObject.FindGameObjectWithTag("Player2").transform.position = Targetpoint;
        }
    }
}

