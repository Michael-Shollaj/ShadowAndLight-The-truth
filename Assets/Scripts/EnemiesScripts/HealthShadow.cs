using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShadow : MonoBehaviour
{
    public int health;
    public GameObject deathEffect;
    public GameObject soulEffect; // Effect for when the shadow turns into a soul
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip changeToSoul;


    private float lightExposureTime = 0f;
    private bool isInLight = false;
    private bool isRevealed = false;
    private bool isGoodShadow = false;

    void Start()
    {
        // Determine if the shadow is good or bad based on its tag
        isGoodShadow = gameObject.CompareTag("GoodShadow");
    }

    void Update()
    {
        if (isInLight)
        {
            lightExposureTime += Time.deltaTime;

            if (lightExposureTime >= 3f && !isRevealed)
            {
                RevealShadowNature();
            }
        }

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LightTrigger"))
        {
            isInLight = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LightTrigger"))
        {
            isInLight = false;
            lightExposureTime = 0f;
        }
    }

    void RevealShadowNature()
    {
        isRevealed = true;

        if (isGoodShadow)
        {

            SoundFXManager.instance.PlaySoundFXClip(changeToSoul, transform, 1f);
            // Turn into a soul and join the player
            Instantiate(soulEffect, transform.position, Quaternion.identity);
            // Implement logic for joining the player
            Destroy(gameObject);
        }
        else
        {
            // Stay as an evil shadow
            // Implement additional logic for evil shadow here
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInLight)
        {
            health -= damage;
            SoundFXManager.instance.PlaySoundFXClip(hurtSound, transform, 1f);
        }
    }
}
