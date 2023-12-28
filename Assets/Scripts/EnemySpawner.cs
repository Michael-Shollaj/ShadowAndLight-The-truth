using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab here
    public Text enemySpawnText;    // Assign your UI Text element here
    public float spawnDelay = 10f; // Delay before the enemy spawns

    private void Start()
    {
        enemyPrefab.SetActive(true);
        enemySpawnText.gameObject.SetActive(false);
        StartCoroutine(SpawnEnemyAfterDelay());
    }

    private IEnumerator SpawnEnemyAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        enemyPrefab.SetActive(true);

        enemySpawnText.gameObject.SetActive(true);
        enemySpawnText.text = "An shadow has entered the scene!";

        // Optional: Hide the text after a few seconds
        yield return new WaitForSeconds(5);
        enemySpawnText.gameObject.SetActive(false);
    }
}
