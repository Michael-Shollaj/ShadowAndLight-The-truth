using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int numOfHearts;
    public Animator anim;
    public GameObject HurtEffect;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private bool isDeathByLight;
    public GameObject DeathPanel;
    [SerializeField] private AudioClip DeathFX;
    [SerializeField] private AudioClip SpikeFX;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        DeathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void AddHealth(int value)
    {
        health += value;
        health = Mathf.Min(health, numOfHearts); // Ensure health doesn't exceed max

        // Update UI
        UpdateHealthUI();
    }


    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < numOfHearts;
        }
    }

    public void TakeDamage(int damage, bool isLightDamage = false)
    {
        health -= damage;
        SoundFXManager.instance.PlaySoundFXClip(SpikeFX, transform, 1f);
        Instantiate(HurtEffect, transform.position, Quaternion.identity);
        StartCoroutine(DamageAnimation());
        isDeathByLight = isLightDamage; // Update the death cause

        if (health <= 0)
        {
            if (isLightDamage)
            {
                HandleGameOver();
            }
            else
            {
                StartCoroutine(HandleDeath());
            }
        }
    }

    private void HandleGameOver()
    {
        anim.SetTrigger("death");
        DeathPanel.SetActive(true);
    }

    private IEnumerator HandleDeath()
    {
        anim.SetTrigger("death"); // Play death animation
        SoundFXManager.instance.PlaySoundFXClip(DeathFX, transform, 1f);
        SoundFXManager.instance.PlaySoundFXClip(SpikeFX, transform, 1f);
        yield return new WaitForSeconds(.1f); // Wait for the animation to finish
        anim.Play("Idle");
        // After animation, call GameManager to respawn player
        GameManager.instance.RespawnPlayer();
    }

    public void ResetHealth()
    {
        health = numOfHearts;
        UpdateHealthUI();
    }

    IEnumerator DamageAnimation()
    {
        {
            SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < 3; i++)
            {
                foreach (SpriteRenderer sr in srs)
                {
                    Color c = sr.color;
                    c.a = 0;
                    sr.color = c;
                }

                yield return new WaitForSeconds(.1f);

                foreach (SpriteRenderer sr in srs)
                {
                    Color c = sr.color;
                    c.a = 1;
                    sr.color = c;
                }
                yield return new WaitForSeconds(.1f);
            }
        }
    }

}
