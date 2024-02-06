using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Wall;
    public PlayerMovement player;
    [SerializeField] private AudioClip DarkVoice;

    private void Start()
    {
        Boss.SetActive(false);
        Wall.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossUI.instance.BossActivator();
            Wall.SetActive(true);
            StartCoroutine(WaitForBoss());
        }
    }

    IEnumerator WaitForBoss()
    {
        Boss.SetActive(false);
        SoundFXManager.instance.PlaySoundFXClip(DarkVoice, transform, 1f);
        yield return new WaitForSeconds(18f);
        Boss.SetActive(true);
        Destroy(gameObject);

    }
}
