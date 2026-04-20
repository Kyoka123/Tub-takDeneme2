using UnityEngine;

public class Death : MonoBehaviour
{
    public Vector3 Targetpoint = new Vector3(0, 2, 0);
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
           GameObject.FindGameObjectWithTag("Player").transform.position = Targetpoint;
        }
        if (other.gameObject.tag == "Player2")
        {
            GameObject.FindGameObjectWithTag("Player2").transform.position = Targetpoint;
        }
    }
}

