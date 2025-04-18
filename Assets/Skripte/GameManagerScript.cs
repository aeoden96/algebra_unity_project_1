using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public List<GameObject> NPCs;
    public float spawnInterval = 2f; // Svako toliko sekundi se spawnaju novo vozilo
    public Transform spawnPoint; //Mjesto na kojem se pojavljuju vozila
    public float moveSpeed = 2f; // Brzina kretanja vozila



    void Start()
    {
        StartCoroutine(SpawnNPCs());
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
        if(NPCs.Count == 0 || spawnPoint == null)
        {
            Debug.LogWarning("Nema dostupnih NPC-ova ili spawn point nije postavljen.");
            return;
        }

        //odabiramo nasumični index iz liste NPC-ova
        int index = Random.Range(0, NPCs.Count);
        //odaberamo NPC iz liste na rednom broju "index"
        GameObject selectedNpc = NPCs[index];
        //Instantiramo NPC na spawn pointu
        GameObject npcInstance = Instantiate(selectedNpc, spawnPoint.position, Quaternion.identity);

        npcInstance.AddComponent<MoveUp>().speed = moveSpeed; // Dodajemo komponentu za pomicanje prema gore
    }
}
public class MoveUp : MonoBehaviour
{
    public float speed = 2f;
    void Update()
    {
        // Pomjeramo objekt prema gore
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
