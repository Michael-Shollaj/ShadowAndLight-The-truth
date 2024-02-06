using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSouls : MonoBehaviour
{
    public GameObject soulPrefab; // Assign in the inspector
    public int maxSoulsCount = 10; // Maximum number of souls to be released
    private int releasedSoulsCount = 0; // Counter for the number of released souls
    private float releaseRadius = 1.0f; // Distance from the player to spawn the souls

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && releasedSoulsCount < maxSoulsCount)
        {
            ReleaseSoul();
        }
    }

    void ReleaseSoul()
    {
        Vector2 spawnPosition = transform.position + (Vector3)(Random.insideUnitCircle * releaseRadius);
        Instantiate(soulPrefab, spawnPosition, Quaternion.identity);
        releasedSoulsCount++; // Increment the counter for each soul released
    }
}
