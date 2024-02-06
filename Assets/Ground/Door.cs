using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerMovement thePlayer;
    public bool doorOpen, waitingToOpen;
    public GameObject Effect;
    [SerializeField] private AudioClip DoorOpenFX;

    public Animator anim;

    // Start is called before the first frame update

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();
        Effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToOpen)
        {
            if(Vector3.Distance(thePlayer.followingKey.transform.position, transform.position) < 0.1f)
            {
                waitingToOpen = false;
                doorOpen = true;
                anim.SetTrigger("Open");
                thePlayer.followingKey.gameObject.SetActive(false);
                SoundFXManager.instance.PlaySoundFXClip(DoorOpenFX, transform, 1f);
                thePlayer.followingKey = null;
                Effect.SetActive(true);
                Debug.Log("DoorOpen");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(thePlayer.followingKey != null)
            {
                thePlayer.followingKey.followTarget = transform;
                waitingToOpen = true;
            }
        }
    }
}
