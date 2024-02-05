using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float speed = 5f;
    private Transform playerTransform;

    private void Start()
    {
        // Find the player in the scene. Adjust this if you have a specific way to reference the player.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            // Optionally, destroy the soul object when it reaches close enough to the player
            if (Vector2.Distance(transform.position, playerTransform.position) < 0.1f)
            {
                GameManager.instance.AddSoul();
                Destroy(gameObject);
            }
        }
    }
}
