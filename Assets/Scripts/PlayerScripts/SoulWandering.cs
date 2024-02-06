using UnityEngine;

public class SoulWandering : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Vector2 moveDirection;
    public float changeDirectionInterval = 2.0f;
    private float timer;

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > changeDirectionInterval)
        {
            ChangeDirection();
            timer = 0;
        }
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void ChangeDirection()
    {
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
