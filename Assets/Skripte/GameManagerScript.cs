using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Helpers;

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

    private void PositionPanel(GameObject panel)
    {
        // Status text
        panelText = panel.transform.Find("Main Text").GetComponent<TextMeshProUGUI>();
        DefineTextRect(panelText);

        // Panel background
        panelImage = panel.GetComponent<Image>();

        // Navigation button
        buttonText = panel.transform.Find("NavigationBtn/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

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
        PositionPanel(panel);
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
        if (spawnPoints == null || !spawnPoints.Where(y => y != null).Any())
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
                ActivatePanelScreen("You are finished", Color.green, Color.black, $"Go to level {NextLevelLabel()}");
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

    private int NextLevelIndex()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex < Scenes.FirstLevel)
        {
            return Scenes.FirstLevel;
        }


        int numberOfLevels = Scenes.LastLevel - Scenes.FirstLevel + 1;

        // Only assumption is that the levels are in order & sequential
        int nextIndex = (currentIndex - Scenes.FirstLevel + 1) % numberOfLevels + Scenes.FirstLevel;

        return nextIndex;
    }

    private int NextLevelLabel()
    {
        return NextLevelIndex() - Scenes.FirstLevel + 1;
    }


    public void LoadLevel()
    {

        if (playerIsDead)
        {
            SceneManager.LoadScene(Scenes.FirstLevel);
            return;
        }

        SceneManager.LoadScene(NextLevelIndex());
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene(Scenes.LevelSelect);

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
