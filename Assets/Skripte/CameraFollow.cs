using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject; // Objekt koji pratimo

    [Tooltip("Orthographic size kada auto miruje (bliže)")]
    public float minZoom = 3f;

    [Tooltip("Orthographic size pri (ili iznad) maxSpeedForZoom (dalje)")]
    public float maxZoom = 6f;

    [Tooltip("Brzina vozila pri kojoj se postiže maxZoom")]
    public float maxSpeedForZoom = 5f;

    [Tooltip("Trajanje prijelaza zooma (koristi SmoothDamp)")]
    public float zoomSmoothTime = 0.2f;

    private Camera cam; //Referenca na kameru
    private float zoomVelocity;  // Interni parametar za SmoothDamp


    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow: uključite orthographic za 2D igru.");
        }

    }


    void Start()
    {
        UpdateCameraPosition(true);       // Init pozicija
        UpdateZoom(true);                 // Init zoom (instant)
    }
    void LateUpdate()
    {
        UpdateCameraPosition();           // Prati x,y targeta
        UpdateZoom();                     // Podesi zoom prema brzini
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

}
