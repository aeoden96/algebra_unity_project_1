using System;
using UnityEngine;

public class Kretanje : MonoBehaviour
{

    [SerializeField]
    float brzinaKretanja = 5f;
    public float brzinaRotiranja = 100f;
    public bool vrtiSeUKrug = false;


    Stanje stanje = Stanje.Neaktivno;
    void Start()
    {
        stanje = Stanje.Aktivno;

    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log($"Stanje: {DateTime.Now.ToString("dd.MM.yyyy HH.mm")}");

        stanje = Stanje.IzvrsavaSe;
        VoziUKrug();
        Pomicanje();
        Rotiraj();
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

