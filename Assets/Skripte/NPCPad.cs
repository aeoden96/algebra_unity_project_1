using UnityEngine;

public class NPCPad : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Trigger entered with: " + collision.gameObject.name);
        Destroy(collision.gameObject); 
    }

}

