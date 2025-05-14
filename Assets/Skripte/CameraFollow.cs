using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject; // Objekt koji pratimo
    private PlayerManager targetScript; // Referenca na MoveScript



    [Tooltip("Orthographic size kada auto miruje (bliže)")]
    public float minZoom = 3f;

    [Tooltip("Orthographic size pri (ili iznad) maxSpeedForZoom (dalje)")]
    public float maxZoom = 6f;

    [Tooltip("Brzina vozila pri kojoj se postiže maxZoom")]
    public float maxSpeedForZoom = 5f;

    [Tooltip("Trajanje prijelaza zooma (koristi SmoothDamp)")]
    public float zoomSmoothTime = 0.2f;

    [Tooltip("Aktivira lagani shake efekt kamere")]
    public bool shakeCamera = false;

    [Tooltip("Trajanje shake efekta u sekundama")]
    public float shakeDuration = 0.3f;

    [Tooltip("Intenzitet pomicanja kamere pri shake efektu")]
    public float shakeMagnitude = 0.1f;

    private Camera cam; //Referenca na kameru
    private float zoomVelocity;  // Interni parametar za SmoothDamp

    private Vector3 originalPos;
    private float shakeTimer = 0f;
    private int lastHitCount = 0;



    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetScript = targetObject.GetComponent<PlayerManager>(); // Referenca na objekt koji pratimo


        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow: uključite orthographic za 2D igru.");
        }

    }


    void Start()
    {
        if (targetObject == null)
        {
            targetObject = GameObject.FindGameObjectWithTag("Player");
            targetScript = targetObject.GetComponent<PlayerManager>();
        }


        UpdateCameraPosition(true);       // Init pozicija
        UpdateZoom(true);                 // Init zoom (instant)

    }
    void LateUpdate()
    {

        if (targetScript.onGrass && targetScript.trenutnaBrzina.magnitude > 0.1f)
        {
            shakeCamera = true;
        }

        else
        {
            shakeCamera = false;
        }



        HandlePlayrHit();
        UpdateCameraPosition();           // Prati x,y targeta
        UpdateZoom();                     // Podesi zoom prema brzini
        HandleCameraShake();
    }




    private void HandlePlayrHit()
    {
        if (lastHitCount != targetScript.brojUdaraca)
        {
            lastHitCount = targetScript.brojUdaraca;
            shakeCamera = true; // Pokreće shake efekt
        }
    }

    //private void SetCameraPosition()
    //{
    //    if (targetObject != null)
    //    {
    //        Vector2 targetPosition = targetObject.transform.position;
    //        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z); // Postavljamo kameru iznad objekta
    //    }
    //    else
    //    {
    //        Debug.LogError("Target object nije postavljen!");
    //    }
    //}


    void UpdateCameraPosition(bool instant = false)
    {
        if (targetObject == null)
        {
            return;
        }
        Vector3 pos = targetObject.transform.position;
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }


    /// <summary>
    /// Dinamički prilagođava orthographicSize kamere na temelju brzine.
    /// </summary>
    void UpdateZoom(bool instant = false)
    {
        if (cam == null || targetObject == null)
        {
            return;
        }


        float speed = 0f;
        if (targetObject.TryGetComponent<Rigidbody2D>(out var rb))
        {
            speed = rb.velocity.magnitude;
        }


        // Koristimo Lerp da brzinu (0→max) prevedemo u zoom (min→max)
        float t = Mathf.Clamp01(speed / maxSpeedForZoom);
        float targetSize = Mathf.Lerp(minZoom, maxZoom, t);

        if (instant)
        {
            cam.orthographicSize = targetSize; // trenutni skok
        }
        else
        {
            // SmoothDamp za uglađeno približavanje vrijednosti
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize,
                                                    targetSize,
                                                    ref zoomVelocity,
                                                    zoomSmoothTime);
        }



    }

    void HandleCameraShake()
    {
        if (shakeCamera)
        {
            // Resetira bool kako bi se shake dogodio samo jednom
            shakeCamera = false;

            // Postavlja trajanje shake efekta na početnu vrijednost
            shakeTimer = shakeDuration;

            // Sprema trenutnu poziciju kamere kako bi se mogla vratiti nakon shake-a
            originalPos = transform.position;
        }

        if (shakeTimer > 0)
        {
            // Generira nasumičan X pomak unutar raspona [-1, 1] * intenzitet
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;

            // Generira nasumičan Y pomak unutar raspona [-1, 1] * intenzitet
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            // Postavlja kameru na originalnu poziciju + nasumičan pomak
            transform.position = new Vector3(
                originalPos.x + offsetX,
                originalPos.y + offsetY,
                originalPos.z
            );

            // Odbrojava trajanje shake efekta
            shakeTimer -= Time.deltaTime;

            // Ako je shake završio, vrati kameru točno na originalnu poziciju
            if (shakeTimer <= 0)
            {
                transform.position = originalPos;
            }

        }



    }
}
