using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    public GameObject panel;
    private Image panelImage;
    private void Awake()
    {
        panelImage = panel.GetComponent<Image>();

        for (int i = Scenes.FirstLevel; i <= Scenes.LastLevel; i++)
        {

            int levelLabel = i - Scenes.FirstLevel + 1;
            int levelIndex = i;

            GameObject buttonPrefab = Resources.Load<GameObject>("Button");
            if (buttonPrefab == null)
            {
                Debug.LogError("Button prefab not found in Resources folder.");
                return;
            }

            GameObject buttonInstance = Instantiate(buttonPrefab, panel.transform);

            buttonInstance.name = "LevelButton" + levelLabel;

            RectTransform buttonRect = buttonInstance.GetComponent<RectTransform>();
            if (buttonRect == null)
            {
                Debug.LogError("Button RectTransform component not found.");
                return;
            }

            float offsetY = -((levelLabel - 1) * 50);
            buttonRect.anchoredPosition = new Vector2(0, offsetY);
            buttonRect.sizeDelta = new Vector2(200, 50);


            TextMeshProUGUI buttonText = buttonInstance.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Level " + levelLabel;
            }
            else
            {
                Debug.LogError("Button text component not found in button prefab.");
            }


            Button button = buttonInstance.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SceneManager.LoadScene(levelIndex));
            }
            else
            {
                Debug.LogError("Button component not found in button prefab.");
            }


        }

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }
}
