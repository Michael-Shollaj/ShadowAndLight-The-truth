using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossScript : MonoBehaviour
{
    public Animator anim;
    public Transform[] transforms;
    public GameObject flame;

    public float timeToShoot, countdown;
    public float timeToTP, countdownToTP;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = transforms[1].position; // Assuming you want the boss to start at a specific position
        countdown = timeToShoot;
        countdownToTP = timeToTP; // Start the teleport countdown
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Countdowns();
        // Removed the direct call to Teleport() from here
    }

    public void Teleport()
    {
        var initialPosition = Random.Range(0, transforms.Length);
        transform.position = transforms[initialPosition].position;
        anim.SetTrigger("Teleport");
    }

    public void Countdowns()
    {
        countdown -= Time.deltaTime;
        countdownToTP -= Time.deltaTime;

        if (countdown <= 0f)
        {
            ShootPlayer();
            countdown = timeToShoot; // Reset the shooting countdown
        }

        if (countdownToTP <= 0f)
        {
            Teleport(); // Only teleport when the countdown reaches zero
            countdownToTP = timeToTP; // Reset the teleport countdown
        }
    }

    public void ShootPlayer()
    {
        anim.SetTrigger("Shoot");
        GameObject spell = Instantiate(flame, transform.position, Quaternion.identity);
        // Consider aiming the spell towards the player here
    }

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); // Use localScale to flip instead of rotation for consistency
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}
