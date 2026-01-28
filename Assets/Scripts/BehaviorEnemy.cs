
using System;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    AGGRO,
    ATTACKING,
    DYING
}

public class EnemyBehavior : MonoBehaviour
{

    public EnemyState state;

    private Vitals enemyVitals;
    private int aggroResetTimer = 0;

    public int aggroCooldown;
    public float viewDistance = 15f; // Max distance the enemy can see
    public float viewAngle = 90f;    // Field of view angle (e.g., 90 degrees)
    public LayerMask obstacleMask;

    private Transform player;

    private bool isRotating;
    private Quaternion targetRotation;
    private float rotationSpeed;

    public float idleRotationSpeed = 1f;

    private NavMeshAgent agent;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyVitals = GetComponent<EnemyVitals>();
        agent = GetComponent<NavMeshAgent>();
        state = EnemyState.IDLE;
        rotationSpeed = idleRotationSpeed;
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player GameObject has the 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyVitals.IsAlive())
        {

            //Idly turn around
            if (state == EnemyState.IDLE)
            {
                rotationSpeed = idleRotationSpeed;
                RandomRotate();
            }

            //Check if can see player and aggro
            if (player != null)
            {
                CheckForPlayerLineOfSight();
            }

            //follow player if aggroed, otherwise stop
            if (state == EnemyState.AGGRO)
            {
                agent.SetDestination(target.position);
                agent.isStopped = false;
            }
            else
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            //Aggro cooldown
            if (aggroResetTimer > 0 && state == EnemyState.AGGRO)
            {
                aggroResetTimer -= 1;
            }
            else if (aggroResetTimer <= 0)
            {
                aggroResetTimer = 0;
                state = EnemyState.IDLE;
            }

            //Attack if within range, oherwise retrn to aggro
            if (GetRemainingDistance() <= agent.stoppingDistance && state == EnemyState.AGGRO)
            {
                state = EnemyState.ATTACKING;
            }
            else if (GetRemainingDistance() > agent.stoppingDistance && state == EnemyState.ATTACKING)
            {
                state = EnemyState.AGGRO;
            }

            //Always face the player if aggroed
            if (state == EnemyState.AGGRO || state == EnemyState.ATTACKING)
            {
                rotationSpeed = idleRotationSpeed * 2.5f;
                RotateToPlayer();
            }
        }
        else
        {
            if (state != EnemyState.DYING)
            {
                Debug.Log("I'm dead");
                state = EnemyState.DYING;
                enemyVitals.Die();
            }
        }


    }

    void Rotate()
    {
        // Smoothly interpolate to the target rotation
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Stop rotating when close enough to target
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void RandomRotate()
    {
        // 1 in 1000 chance per frame (approx 60 times a second if 60fps)
        if (!isRotating && UnityEngine.Random.Range(0, 200) == 0)
        {
            // Create a new random Euler angle (e.g., around Y axis)
            float randomY = UnityEngine.Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(0, randomY, 0);
            isRotating = true;
        }

        Rotate();
    }

    void CheckForPlayerLineOfSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        // Normalize the direction vector for the angle calculation
        Vector3 normalizedDirection = directionToPlayer.normalized;

        // 1. Check if player is within range
        if (distanceToPlayer < viewDistance)
        {
            // 2. Check if player is within the enemy's field of view angle
            if (Vector3.Angle(transform.forward, normalizedDirection) < viewAngle / 2f)
            {
                RaycastHit hit;
                // 3. Cast a ray to check for obstacles
                if (Physics.Raycast(transform.position, normalizedDirection, out hit, viewDistance))
                {

                    // Check if the first object hit is the player
                    if (hit.collider.CompareTag("Player"))
                    {
                        Aggro();
                    }
                }
            }
        }
    }

    public float GetRemainingDistance()
    {
        if (agent.pathPending || agent.pathStatus == NavMeshPathStatus.PathInvalid || agent.path.corners.Length == 0)
            return -1f; // Path is not ready or invalid

        float distance = 0.0f;
        // Start from current agent position to the next corner
        for (int i = 0; i < agent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(agent.path.corners[i], agent.path.corners[i + 1]);
        }

        // Return total calculated distance
        return distance;
    }

    void RotateToPlayer()
    {
        targetRotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up)
        );
        if (Quaternion.Angle(transform.rotation, targetRotation) >= 0.1f)
            isRotating = true;
        Rotate();
    }

    // Optional: Draw the line of sight in the Scene view for debugging
    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = aggroResetTimer == aggroCooldown ? Color.red : Color.yellow;
            Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * viewDistance);
        }
    }

    public void Aggro()
    {
        state = EnemyState.AGGRO;
        aggroResetTimer = aggroCooldown;
    }
}
