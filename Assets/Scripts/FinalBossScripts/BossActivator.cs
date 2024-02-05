using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject Boss;
    public GameObject Wall;

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
            Boss.SetActive(true);
            Wall.SetActive(true);
        }
    }
}
