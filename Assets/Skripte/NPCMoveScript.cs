using UnityEngine;

public class NPCMoveScript : MonoBehaviour
{
    public DirectionFlow directionFlow; // Enum za definiranje smjera kretanja
    public float speed = 2f; // Brzina kretanja NPC-a

    // Start is called before the first frame update
    void Start()
    {

        switch (directionFlow)
        {
            case DirectionFlow.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0); // Postavi rotaciju na 0 stupnjeva
                break;
            case DirectionFlow.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case DirectionFlow.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case DirectionFlow.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }


    private void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }
}

    // Update is called once per frame
    //void Update()
    //{
    //    switch(directionFlow)
    //    {
    //        case DirectionFlow.Up:
    //            transform.Translate(Vector2.up * speed * Time.deltaTime);
    //            break;
    //        case DirectionFlow.Down:
    //            transform.Translate(Vector2.down * speed * Time.deltaTime);
    //            break;
    //        case DirectionFlow.Left:
    //            transform.Translate(Vector2.left * speed * Time.deltaTime);
    //            break;
    //        case DirectionFlow.Right:
    //            transform.Translate(Vector2.right * speed * Time.deltaTime);
    //            break;
    //    }
    //}

