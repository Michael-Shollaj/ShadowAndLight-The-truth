using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Player Following")]
    [SerializeField] private float followRange;
    [SerializeField] private Transform player;
    private bool isFollowingPlayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        initScale = enemy.localScale;
        rb = enemy.GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            Patrol();
        }

        float distanceToPlayer = Vector3.Distance(enemy.position, player.position);
        if (distanceToPlayer < followRange)
        {
            isFollowingPlayer = true;
        }
        else if (distanceToPlayer > followRange)
        {
            isFollowingPlayer = false;
        }
    }

    private void Patrol()
    {
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0;
        }
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetBool("moving", true);

        // Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        // Move in that direction using Rigidbody2D
        Vector2 newPosition = new Vector2(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y);
        rb.MovePosition(newPosition);
    }

    private void FollowPlayer()
    {
        anim.SetBool("moving", true);

        // Determine direction to face
        int direction = player.position.x > enemy.position.x ? 1 : -1;
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        // Move towards player using Rigidbody2D
        Vector2 newPosition = Vector2.MoveTowards(enemy.position, new Vector3(player.position.x, enemy.position.y, enemy.position.z), speed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }
}
