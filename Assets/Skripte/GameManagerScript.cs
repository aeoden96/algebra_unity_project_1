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
    public float spawnInterval = 2f;
    public List<GameObject> spawnPoints = null;
    public GameObject panel;

    bool playerIsFinished = false;
    bool playerIsDead = false;

    private TextMeshProUGUI panelText;
    private Image panelImage;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag(UiTags.MainPanel);
        }

        if (panel == null)
        {
            return;
        }

        panel.SetActive(false);

        panelText = panel.transform.Find("Main Text").GetComponent<TextMeshProUGUI>();
        panelImage = panel.GetComponent<Image>();
        buttonText = panel.transform.Find("NavigationBtn/Text (TMP)").GetComponent<TextMeshProUGUI>();

        if (panelText != null)
        {
            RectTransform textRect = panelText.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 1f);
            textRect.anchorMax = new Vector2(0.5f, 1f);
            textRect.pivot = new Vector2(0.5f, 1f);
            textRect.anchoredPosition = new Vector2(0, -40);
            textRect.sizeDelta = new Vector2(600, 60);
            panelText.enableWordWrapping = false;
            panelText.alignment = TextAlignmentOptions.Center;
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
        if(spawnPoints == null || !spawnPoints.Where(y=>y != null).Any())
        {
            return;
        }



        StartCoroutine(SpawnNPCs());
    }

    void LateUpdate()
    {
        if (playerIsFinished || playerIsDead)
        {
            panel.SetActive(true);

            if (playerIsDead)
            {
                ActivatePanelScreen("You are dead", Color.red, Color.black, "Try Again");
            }

            if (playerIsFinished)
            {
                ActivatePanelScreen("You are finished", Color.green, Color.black, "Next Level");
            }
        }
    }

    private void ActivatePanelScreen(string msg, Color backgourndText, Color textColor, string buttonLabel)
    {
        if (panelImage != null) panelImage.color = backgourndText;

        if (panelText != null)
        {
            panelText.text = msg;
            panelText.color = textColor;
        }

        if (buttonText != null)
        {
            buttonText.text = buttonLabel;
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }


    public void LoadLevel()
    {
        if (playerIsDead)
        {
            SceneManager.LoadScene(Scenes.Lvl1);
            return;
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
        }
        SceneManager.LoadScene(nextIndex);
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
            int index = Random.Range(0, NPCs.Count);
            GameObject selectedNpc = NPCs[index];
            var directionFlow = spawnPoint.GetComponent<NPCStarPositionManager>().directionFlow;

            GameObject npcInstance = Instantiate(selectedNpc, spawnPoint.transform.position, Quaternion.identity);
            npcInstance.AddComponent<NPCMoveScript>().directionFlow = directionFlow;
        }
    }
}
