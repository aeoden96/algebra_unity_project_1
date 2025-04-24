using TMPro;
using UnityEngine;

public class CanvasHealthManager : MonoBehaviour
{

    public GameObject playerVehicle;
    private TextMeshProUGUI meshProUGUI;
    private MoveScript moveScript;
    // Start is called before the first frame update
    void Start()
    {
        meshProUGUI = GetComponent<TextMeshProUGUI>();
        if (playerVehicle == null)
        {
            meshProUGUI.text = "Vehicle is null!!";
            return;
        }

        moveScript = playerVehicle.GetComponent<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerVehicle == null)
        {
            return;
        }

        if (moveScript.dozvoliKretanje)
        {
            meshProUGUI.text = $"Health: {moveScript.zdravlje}";
            return;
        }

        meshProUGUI.color = Color.red;
        meshProUGUI.text = $"R.I.P.";
    }
}
