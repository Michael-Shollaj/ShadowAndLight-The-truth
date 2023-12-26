using UnityEngine;
using System.Collections;
public class PlayerCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Assuming each collision decreases health by 1
            }
        }
    }
}