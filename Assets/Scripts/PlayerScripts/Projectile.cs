using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitEffect;
    public float DestroyEffect;
    public float DestroyBullet;
    public int damage; // Damage that this projectile will deal
    [SerializeField] private AudioClip Hit;

    void Update()
    {
        Destroy(gameObject, DestroyBullet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, DestroyEffect);
        SoundFXManager.instance.PlaySoundFXClip(Hit, transform, 1f);

        // Check if the object we collided with has a HealthShadow component
        HealthShadow health = collision.GetComponent<HealthShadow>();
        if (health != null)
        {
            // If it does, deal damage
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

}
