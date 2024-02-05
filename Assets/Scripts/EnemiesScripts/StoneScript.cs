using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public GameObject Smoke; // Assign this in the inspector

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the stone hits the "Ground"
        if (collision.gameObject.tag == "Ground")
        {
            Instantiate(Smoke, transform.position, Quaternion.identity);
        }
    }


}
