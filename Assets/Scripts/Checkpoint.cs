using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 LastCheckpointPosition;
    public GameObject Fire;

    private void Start()
    {
        Fire.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LastCheckpointPosition = transform.position;
            Fire.SetActive(true);
        }
    }
}
