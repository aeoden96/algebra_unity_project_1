using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public List<GameObject> NPCs;
    public float spawnInterval = 2f; // Svako toliko sekundi se spawnaju novo vozilo
    public List<GameObject> spawnPoints; //Mjesta na kojem se pojavljuju vozila
    public GameObject panel; // Referenca na UI panel za editiranje
    bool playerIsFinished = false;
    bool playerIsDead = false; // Da li je igrač mrtav

    public TextMeshProUGUI panelText;
    private Image panelImage;
    public TextMeshProUGUI buttonText;


    private void Awake()
    {
        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag(UiTags.MainPanel);
        }

        panel.SetActive(false); // Skriveno na početku
        panelText = panel.GetComponentInChildren<TextMeshProUGUI>();
        panelImage = panel.GetComponentInChildren<Image>();
        buttonText = panel.transform.Find("NavigationBtn/Text (TMP)").GetComponent<TextMeshProUGUI>();


        if (panelText != null)
        {
            RectTransform textReac = panelText.GetComponent<RectTransform>();
            textReac.anchorMin = new Vector2(0.5f, 1f);
            textReac.anchorMax = new Vector2(0.5f, 1f);
            textReac.pivot = new Vector2(0.5f, 1f);
            textReac.anchoredPosition = new Vector2(0, -40);
            textReac.sizeDelta = new Vector2(600, 60);

            panelText.enableWordWrapping = false;
            panelText.alignment = TextAlignmentOptions.Center;
        }


    }

    void ActivatePanelScreen(string msg, Color txtColor, Color bcgColor, string buttonLabel)
    {
        panel.SetActive(true);

        if (panelImage != null)
        {
            panelImage.color = bcgColor;
        }
        if (panelText != null)
        {
            panelText.text = msg;
            panelText.color = txtColor;

        }

        if (buttonText != null)
        {
            buttonText.text = buttonLabel;

        }

    }


    public void PlayerFinished()
    {
        playerIsFinished = true;
    }

    public void PlayerDead()
    {
        playerIsDead = true;
    }



    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    void LateUpdate()
    {
        if (playerIsFinished || playerIsDead)
        {
            if (playerIsDead)
            {
                ActivatePanelScreen("You are dead!", Color.white, Color.red, "Try Again");
            }

            if (playerIsFinished)
            {
                ActivatePanelScreen("You are won!", Color.white, Color.green, "Next Level");
            }



        }
    }


    public void LoadLevel()
    {
        if (playerIsDead)
        {
            SceneManager.LoadScene(0);
        }

        if (playerIsFinished)
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextIndex = 0;
            }
            SceneManager.LoadScene(nextIndex);
        }





    }


    IEnumerator SpawnNPCs()
    {
        while (true)
        {
            SpawnRandomNPC();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    void SpawnRandomNPC()
    {
        if (NPCs.Count == 0 || (spawnPoints == null))
        {
            Debug.LogWarning("Nema dostupnih NPC-ova ili spawn point nije postavljen.");
            return;
        }

        if (!spawnPoints.Any())
        {
            Debug.LogWarning("Nema dostupnih spawn pointova.");
            return;
        }


        foreach (var spawnPoint in spawnPoints)
        {

            //odabiramo nasumični index iz liste NPC-ova
            int index = Random.Range(0, NPCs.Count);
            //odaberamo NPC iz liste na rednom broju "index"
            GameObject selectedNpc = NPCs[index];
            var directionFlow = spawnPoint.GetComponent<NPCStarPositionManager>().directionFlow; // Dobijamo smjer kretanja iz spawn pointa

            //Instantiramo NPC na spawn pointu
            GameObject npcInstance = Instantiate(selectedNpc, spawnPoint.transform.position, Quaternion.identity);

            npcInstance.AddComponent<NPCMoveScript>().directionFlow = directionFlow; // Dodajemo komponentu za pomicanje prema gore
        }


    }
}
