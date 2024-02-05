using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossScript : MonoBehaviour
{
    public Transform[] transforms;
    public GameObject flame;

    public float timeToShoot, countdown;
    public float timeToTP, countdownToTP;

    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = transforms[1].position;
        countdown = timeToShoot;
        countdownToTP = timeToTP;
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Countdowns();
        Teleport();
    }

    public void Teleport()
    {
        var initialPosition = Random.Range(0, transforms.Length);
        transform.position = transforms[initialPosition].position;
    }

    public void Countdowns()
    {
        countdown -= Time.deltaTime;
        countdownToTP -= Time.deltaTime;

        if (countdown <= 0f)
        {
            ShootPlayer();
            countdown = timeToShoot;
        }

        if (countdownToTP <= 0f)
        {
            countdownToTP = timeToTP;
            Teleport();
        }
    }

    public void ShootPlayer()
    {
        GameObject spell = Instantiate(flame, transform.position, Quaternion.identity);
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
