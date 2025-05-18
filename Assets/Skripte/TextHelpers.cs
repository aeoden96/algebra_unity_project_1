using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public static class Helpers
{
  public static void DefineTextRect(TextMeshProUGUI textMeshProUGUI)
  {
    if (textMeshProUGUI == null)
    {
      Debug.LogError("TextMeshProUGUI component is null.");
      return;
    }
    RectTransform textRect = textMeshProUGUI.GetComponent<RectTransform>();
    textRect.anchorMin = new Vector2(0.5f, 1f);
    textRect.anchorMax = new Vector2(0.5f, 1f);
    textRect.pivot = new Vector2(0.5f, 1f);
    textRect.anchoredPosition = new Vector2(0, -40);
    textRect.sizeDelta = new Vector2(600, 60);
    textMeshProUGUI.enableWordWrapping = false;
    textMeshProUGUI.alignment = TextAlignmentOptions.Center;
  }
}
