using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float moveSpeed = 2f;

    private int currentWaypointIndex = 0;

    private void Start()
    {
        if (waypoints.Length > 0)
        {
            // Set the initial position of the Enemy to the first waypoint
            transform.position = waypoints[currentWaypointIndex].transform.position;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypoints.Length == 0) return; // Check if there are waypoints

        // Move the enemy towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position,
            waypoints[currentWaypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the current waypoint
        if (transform.position == waypoints[currentWaypointIndex].transform.position)
        {
            // Select a random waypoint index, different from the current one
            int nextWaypointIndex;
            do
            {
                nextWaypointIndex = Random.Range(0, waypoints.Length);
            }
            while (nextWaypointIndex == currentWaypointIndex);

            currentWaypointIndex = nextWaypointIndex;
        }
    }
}
