using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject GameOverPanel;
    public Text respawnText; // UI Text to display remaining respawns
    public Animator anim;
    public int currentHealth;

    private int deathCount = 0;
    public const int MaxDeaths = 3; // Maximum allowed deaths before game over

    void Start()
    {
        if (Checkpoint.LastCheckpointPosition != Vector3.zero)
        {
            transform.position = Checkpoint.LastCheckpointPosition;
        }
        else
        {
            Checkpoint.LastCheckpointPosition = transform.position;
        }

        GameOverPanel.SetActive(false);
        anim = GetComponent<Animator>();
        UpdateRespawnText();
    }

    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < numOfHearts);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("hurt");
        StartCoroutine(DamageAnimation());

        if (health <= 0)
        {
            if (deathCount < MaxDeaths)
            {
                RespawnPlayer();
                deathCount++;
            }
            else
            {
                GameOver();
            }
        }
    }

    void RespawnPlayer()
    {
        health = numOfHearts;
        transform.position = Checkpoint.LastCheckpointPosition;
        UpdateHeartsUI();
        UpdateRespawnText();
    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0; // Stop the game
        respawnText.enabled = false; // Optionally hide the respawn text on game over
    }

    IEnumerator DamageAnimation()
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

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
            hearts[i].enabled = (i < numOfHearts);
        }
    }

    private void UpdateRespawnText()
    {
        int respawnsLeft = MaxDeaths - deathCount;
        respawnText.text = "x " + respawnsLeft.ToString();
    }
}
