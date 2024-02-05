using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyFace : MonoBehaviour
{
    public GameObject HappySprite;

    // Start is called before the first frame update
    void Start()
    {
        HappySprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EvilShadow") || other.CompareTag("GoodShadow"))
        {
            HappySprite.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EvilShadow") || other.CompareTag("GoodShadow"))
        {
            HappySprite.SetActive(true);
        }
    }
}
