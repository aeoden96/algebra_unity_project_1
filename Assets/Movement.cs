using System;
using UnityEngine;

public class Kretanje : MonoBehaviour
{

    [SerializeField]
    float brzinaKretanja = 5f;
    public float brzinaRotiranja = 100f;
    public bool vrtiSeUKrug = false;


    [SerializeField] private float rotationSpeed = 100f;
    private float maxRotationAngle = 10f; // Maximum rotation angle in degrees
    [SerializeField]
    Vector2 wheelDirection = new Vector2(0, 1);


    Stanje stanje = Stanje.Neaktivno;
    void Start()
    {
        stanje = Stanje.Aktivno;
        rotationSpeed = 100f;

        // initialize wheelDirection with local rotation
        wheelDirection = new Vector2(transform.up.x, transform.up.y);

    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log($"Stanje: {DateTime.Now.ToString("dd.MM.yyyy HH.mm")}");

        //stanje = Stanje.IzvrsavaSe;
        //VoziUKrug();
        //Pomicanje();
        //Rotiraj();

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // if maxRotationAngle is reached, stop rotating
            if (Vector2.SignedAngle(wheelDirection, new Vector2(transform.up.x, transform.up.y)) < -maxRotationAngle)
            {
                float angle = rotationSpeed * Time.deltaTime;
                wheelDirection = RotateVector(wheelDirection, angle);
            }
        }else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // if maxRotationAngle is reached, stop rotating
            if (Vector2.SignedAngle(wheelDirection, new Vector2(transform.up.x, transform.up.y)) > maxRotationAngle)
            {
                float angle = rotationSpeed * Time.deltaTime;
                wheelDirection = RotateVector(wheelDirection, -angle);
            }

        } else
        {
            Vector2 originalDirection = new Vector2(transform.up.x, transform.up.y);
            float maxStep = rotationSpeed * Time.deltaTime;
            wheelDirection = RotateTowards(wheelDirection, originalDirection, maxStep);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += (Vector3)(wheelDirection.normalized * brzinaKretanja * Time.deltaTime);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                float angle = rotationSpeed * Time.deltaTime;
                wheelDirection = RotateVector(wheelDirection, angle);
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + angle);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                float angle = rotationSpeed * Time.deltaTime;
                wheelDirection = RotateVector(wheelDirection, -angle);
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - angle);
            }



        }

        Debug.DrawRay(transform.position, wheelDirection * 20, Color.red);
        Debug.Log($"Wheel Direction: {wheelDirection}");



    }

    private Vector2 RotateTowards(Vector2 current, Vector2 target, float maxDegreesDelta)
    {
        float angle = Vector2.SignedAngle(current, target);
        float rotationAmount = Mathf.Clamp(angle, -maxDegreesDelta, maxDegreesDelta);
        return RotateVector(current, rotationAmount);
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    bool VoziUKrug()
    {

        if (vrtiSeUKrug)
        {
            transform.Rotate(0f, 0f, 0.1f);
            transform.Translate(0f, 0.005f, 0f);
        }





        return true;

    }
    void Pomicanje()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * brzinaKretanja * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * brzinaKretanja * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * brzinaKretanja * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * brzinaKretanja * Time.deltaTime);
        }
    }


    void Rotiraj()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            //transform.Rotate(0f, 0f, -brzinaRotiranja);
            transform.Rotate(0f, 0f, -brzinaRotiranja * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            //transform.Rotate(0f, 0f, brzinaRotiranja);
            transform.Rotate(0f, 0f, brzinaRotiranja * Time.deltaTime);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Sudar!");
        Debug.Log($"Sudario sam se s objektom: {collision.gameObject.name}");
        vrtiSeUKrug = !vrtiSeUKrug;




        VoziUKrug();
    }

}
public enum Stanje
{
    Neaktivno,
    Aktivno,
    IzvrsavaSe
}

