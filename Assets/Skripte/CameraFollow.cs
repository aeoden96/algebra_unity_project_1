using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject; // Objekt koji pratimo
    void Start()
    {
        SetCameraPosition();
    }
    void LateUpdate()
    {
        SetCameraPosition();
    }
    private void SetCameraPosition()
    {
        if (targetObject != null)
        {
            Vector2 targetPosition = targetObject.transform.position;
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z); // Postavljamo kameru iznad objekta
        }
        else
        {
            Debug.LogError("Target object nije postavljen!");
        }
    }
}
