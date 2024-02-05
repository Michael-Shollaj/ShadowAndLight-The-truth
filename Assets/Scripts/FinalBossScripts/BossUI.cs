using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUI : MonoBehaviour
{

    public GameObject bossPanel;

    public static BossUI instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bossPanel.SetActive(false);


    }

    public void BossActivator()
    {
        bossPanel.SetActive(true);

    }
}
