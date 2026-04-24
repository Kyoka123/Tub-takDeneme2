using UnityEngine;

public class Death : MonoBehaviour
{
    Vector3 Targetpoint;
   
    
    private void OnTriggerEnter(Collider deniz)
    {
        Targetpoint = new Vector3(Random.Range(0, 10), 2, Random.Range(5, 15));
        if (deniz.gameObject.tag == "Player")
        {
           GameObject.FindGameObjectWithTag("Player").transform.position = Targetpoint;
        }
        if (deniz.gameObject.tag == "Player2")
        {
            GameObject.FindGameObjectWithTag("Player2").transform.position = Targetpoint;
        }
    }
}

