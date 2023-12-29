using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float moveSpeed = 7f;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float detectionRadius = 5f;
    [SerializeField]
    private Transform specialWaypoint1;
    [SerializeField]
    private Transform specialWaypoint2;

    private Rigidbody2D rb;
    private Transform targetWaypoint;
    private bool isHeadingToSpecialPoint = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetWaypoint = waypoints.Length > 0 ? waypoints[0] : null;
    }

    private void Update()
    {
        if (targetWaypoint == null) return;

        Move();

        if (IsPlayerClose() && !isHeadingToSpecialPoint)
        {
            SwitchToSpecialWaypoint();
        }
    }

    private void Move()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position,
            targetWaypoint.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            targetWaypoint = GetNextWaypoint();
            isHeadingToSpecialPoint = false;
        }
    }

    private bool IsPlayerClose()
    {
        return Vector2.Distance(transform.position, player.position) < detectionRadius;
    }

    private void SwitchToSpecialWaypoint()
    {
        if (targetWaypoint == specialWaypoint1 || targetWaypoint == specialWaypoint2)
        {
            targetWaypoint = specialWaypoint1 == targetWaypoint ? specialWaypoint2 : specialWaypoint1;
        }
        else
        {
            targetWaypoint = Random.Range(0, 2) == 0 ? specialWaypoint1 : specialWaypoint2;
        }
        isHeadingToSpecialPoint = true;
    }

    private Transform GetNextWaypoint()
    {
        if (waypoints.Length == 1)
        {
            return waypoints[0];
        }

        int nextWaypointIndex;
        do
        {
            nextWaypointIndex = Random.Range(0, waypoints.Length);
        }
        while (waypoints[nextWaypointIndex] == targetWaypoint);

        return waypoints[nextWaypointIndex];
    }
}
