using System;
using TMPro;
using UnityEngine;

public class MoveScript : MonoBehaviour
{

    [SerializeField]
    float brzinaKretanja = 5f;

    public float brzinaRotiranja = 100f;
    public bool vrtiSeUKrug = false;
    public float zdravlje = 100f;
    public bool dozvoliKretanje = true;
    public int brojUdaraca = 0;


    public float trenutnaRotBrzina = 0f;
    public float maxRotacijskaBrzina = 150f;
    public float ubrzanjeRotacije = 300f;
    public float usporavanjeRotacije = 200f;
    public float nagloUsporavanje = 25f;




    //Trenutna brzina kretanja vozila
    Vector2 trenutnaBrzina = Vector2.zero;
    public float maxismalnaBrzina = 5f;
    //Brzina kojom se vozilo usporava kada se ne drži tipka
    public float usporavanje = 5f;
    //brzina kojom se vozilo ubrzava prema maksimalnoj brzini
    public float akceleracija = 10f;

    Rigidbody2D rb;
    Stanje stanje = Stanje.Neaktivno;
    float nagloTimer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stanje = Stanje.Aktivno;
    }
    // Update is called once per frame
    void Update()
    {

        //Debug.Log($"Stanje: {DateTime.Now.ToString("dd.MM.yyyy HH.mm")}");

        stanje = Stanje.IzvrsavaSe;

        if (nagloTimer > 0f)
        {
            nagloTimer -= Time.deltaTime;
        }


        if (dozvoliKretanje)
        {
            VoziUKrug();
            Pomicanje();
            Rotiraj();
        }





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
        //Inicijalizacija ulaza
        float ulaz = 0f;

        //prvovjera pritisnutih tipki za naprijed i natrag
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ulaz += 1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            ulaz -= 1f;
        }

        //Ako se pritisne neka od tipki
        if (ulaz != 0)
        {
            //Smjer gibanja ovisi o trenutnoj rotaciji objekta
            Vector2 smjer = transform.up * ulaz;
            //Postepeno ubrzavanje do ciljne brzine
            trenutnaBrzina = Vector2.MoveTowards(trenutnaBrzina, smjer * maxismalnaBrzina, akceleracija * Time.deltaTime);

        }
        else
        {
            //Ako se ne drži tipka/gas, usporavanje prema mirovanju
            trenutnaBrzina = Vector2.MoveTowards(trenutnaBrzina, Vector2.zero, usporavanje * Time.deltaTime);

        }


        rb.velocity = trenutnaBrzina;

        #region Stara verzija kretanja
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.Translate(Vector2.up * brzinaKretanja * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.Translate(Vector2.down * brzinaKretanja * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.Translate(Vector2.left * brzinaKretanja * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.Translate(Vector2.right * brzinaKretanja * Time.deltaTime);
        //}
        #endregion
    }






    void Rotiraj()
    {
        float ulaz = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ulaz += 1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ulaz -= 1f;
        }


        #region Opcionalna rotacija
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            ulaz += 1f;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            ulaz -= 1f;
        }
        #endregion
        if (trenutnaBrzina.magnitude > 0.1f)
        {

            if (ulaz != 0)
            {
                //Postepeno ubrzavanje do ciljne brzine rotacije
                trenutnaRotBrzina = Mathf.MoveTowards(trenutnaRotBrzina, ulaz * maxRotacijskaBrzina, ubrzanjeRotacije * Time.deltaTime);
                Debug.Log($"Ubrzavam rotaciju: {ulaz} trenutnaRotBrzina {trenutnaRotBrzina}"); //Debug.Log($"Ubrzavam rotaciju: {usporavanjeRotacije}");
            }

            else
            {                            //if (ulaz == 0)    //ono sta bi napisali       //ovo bi bio else
                                         // u {}
                float faktorUsporRot = (nagloTimer > 0f) ? usporavanjeRotacije * 2f : usporavanjeRotacije;
                trenutnaRotBrzina = Mathf.MoveTowards(trenutnaRotBrzina, 0f, faktorUsporRot * Time.deltaTime);
                Debug.Log($"Usporavam rotaciju: {faktorUsporRot} trenutnaRotBrzina {trenutnaRotBrzina}"); //Debug.Log($"Usporavam rotaciju: {usporavanjeRotacije}");
            }

            transform.Rotate(0f, 0f, trenutnaRotBrzina * Time.deltaTime);


        }
        else
        {
            trenutnaRotBrzina = 0f;
        }

  


        #region Stara verzija rotacije
        //if (Input.GetKey(KeyCode.KeypadPlus))
        //{
        //    //transform.Rotate(0f, 0f, -brzinaRotiranja);
        //    transform.Rotate(0f, 0f, -brzinaRotiranja * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.KeypadMinus))
        //{
        //    //transform.Rotate(0f, 0f, brzinaRotiranja);
        //    transform.Rotate(0f, 0f, brzinaRotiranja * Time.deltaTime);
        //}
        #endregion
    }


    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Sudar!");
    //    Debug.Log($"Sudario sam se s objektom: { collision.gameObject.name}");
    //    //vrtiSeUKrug = !vrtiSeUKrug;
    //    VoziUKrug(); 
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Sudar!");
        Debug.Log($"Sudario sam se s objektom: {collision.gameObject.name}");
        var preprekaSkripta = collision.gameObject.GetComponent<PreprekaSkripta>();
        var faktorStete = preprekaSkripta.faktorStete;

        if (zdravlje < 1)
        {
            zdravlje = 1;
        }

        zdravlje -= 10f * faktorStete;
        brojUdaraca++; 


        if (zdravlje <= 0)
        {
            dozvoliKretanje = false;
            StartCoroutine(ZamrzniNakonOdbijanja(0.3f));

            //vrtiSeUKrug = true;
            Debug.Log("Totalka!");
        }

        //vrtiSeUKrug = !vrtiSeUKrug;
        VoziUKrug();
    }



    System.Collections.IEnumerator ZamrzniNakonOdbijanja(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }


}
public enum Stanje
{
    Neaktivno,
    Aktivno,
    IzvrsavaSe
}

