using UnityEngine;

public class LightTriggerZone : MonoBehaviour
{
    private CandleLight candleLightScript;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            candleLightScript = player.GetComponent<CandleLight>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && candleLightScript != null)
        {
            candleLightScript.TurnOnLight();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && candleLightScript != null)
        {
            candleLightScript.ExitTriggerZone();
        }
    }
}
