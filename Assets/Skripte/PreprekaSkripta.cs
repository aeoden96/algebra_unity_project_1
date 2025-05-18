using UnityEngine;
using UnityEngine.SceneManagement;

public class PreprekaSkripta : MonoBehaviour
{
  public float faktorStete = 1;


  void Awake()
  {
    int levelIndex = SceneManager.GetActiveScene().buildIndex;

    if (levelIndex == Scenes.Lvl3)
    {
      faktorStete = 3;
    }
  }

}
