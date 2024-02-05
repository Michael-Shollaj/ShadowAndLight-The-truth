using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthValue = 1; // The amount of health this pickup will restore
    public GameObject HealthEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddHealth(healthValue);
                Destroy(gameObject); // Destroy the health pickup object after it's used
                Instantiate(HealthEffect, transform.position, Quaternion.identity);
            }
        }
    }
}
