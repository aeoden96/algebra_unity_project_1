using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public List<GameObject> NPCs;
    public float spawnInterval = 2f; // Svako toliko sekundi se spawnaju novo vozilo
    public List<GameObject> spawnPoints; //Mjesta na kojem se pojavljuju vozila
    public GameObject panel; // Referenca na UI panel za editiranje
    bool playerIsFinished = false;






    private void Awake()
    {
        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag(UiTags.MainPanel);
        }

        panel.SetActive(false); // Skriveno na početku
    }

    public void PlayerFinished()
    {
        playerIsFinished = true;
    }

    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    void LateUpdate()
    {
        if (playerIsFinished)
        {
            //StopAllCoroutines(); // Zaustavi sve korutine
            panel.SetActive(true); // Prikaži UI panel
        }
    }


    public void LoadNextLevel()
    {
        int level = 1;
        int nextLevel = level++;

        Debug.Log($"Učitavam sljedeći level: {nextLevel}");

        SceneManager.LoadScene($"Level 2");
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
