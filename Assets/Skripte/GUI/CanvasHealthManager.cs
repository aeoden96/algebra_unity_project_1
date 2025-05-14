using TMPro;
using UnityEngine;

public class CanvasHealthManager : MonoBehaviour
{

    public GameObject playerVehicle;
    private TextMeshProUGUI meshProUGUI;
    private PlayerManager playerManager;
    // Start is called before the first frame update
    void Start()
    {
        meshProUGUI = GetComponent<TextMeshProUGUI>();
        if (playerVehicle == null)
        {
            meshProUGUI.text = "Vehicle is null!!";
            return;
        }

        playerManager = playerVehicle.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerVehicle == null)
        {
            return;
        }

        if (playerManager.dozvoliKretanje)
        {
            meshProUGUI.text = $"Health: {playerManager.health}";
            return;
        }

        meshProUGUI.color = Color.red;
        meshProUGUI.text = $"R.I.P.";
    }
}
