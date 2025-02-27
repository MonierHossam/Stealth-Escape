using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Detect, Chase, Return, Caught }
    public AIState currentState = AIState.Patrol;

    public Transform[] waypoints;
    public Transform player;

    public float detectionRange = 10f;
    public float chaseRange = 5f;
    public float caughtRange = 1.0f;
    public float visionAngle = 60f;
    public float chaseSpeed = 5f;
    public float patrolSpeed = 2f;
    public float returnTime = 5f;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float lostTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        MoveToNextWaypoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                DetectPlayer();
                break;
            case AIState.Detect:
                DetectPlayer();
                break;
            case AIState.Chase:
                ChasePlayer();
                break;
            case AIState.Return:
                ReturnToPatrol();
                break;
            case AIState.Caught:
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance < detectionRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer.normalized);

            if (angle < visionAngle / 2)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (distance < chaseRange)
                        {
                            currentState = AIState.Chase;
                            agent.speed = chaseSpeed;
                        }
                        else
                        {
                            currentState = AIState.Detect;
                        }
                    }
                }
            }
        }
        else if (currentState == AIState.Detect || currentState == AIState.Chase)
        {
            lostTime += Time.deltaTime;
            if (lostTime >= returnTime)
            {
                currentState = AIState.Return;
            }
        }
    }


    void ChasePlayer()
    {
        agent.destination = player.position;

        if (Vector3.Distance(transform.position, player.position) <= caughtRange)
        {
            Debug.Log("caught");
            currentState = AIState.Caught;
            GameManager.Instance.PlayerCaught();
            return;
        }

        if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            Debug.Log("detect");
            currentState = AIState.Detect;
        }
    }

    void ReturnToPatrol()
    {
        lostTime += Time.deltaTime;
        if (lostTime >= returnTime)
        {
            currentState = AIState.Patrol;
            agent.speed = patrolSpeed;
            MoveToNextWaypoint();
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = currentState == AIState.Chase ? Color.red : Color.green;

        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, leftBoundary * detectionRange);
        Gizmos.DrawRay(transform.position, rightBoundary * detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * detectionRange);
    }
}
