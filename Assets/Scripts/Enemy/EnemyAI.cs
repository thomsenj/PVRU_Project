using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Fusion;
using Fusion.XRShared.Demo;

public class EnemyAI : NetworkBehaviour
{
    [Header("Settings")]
    public string[] targetTags;
    public float moveSpeed = 2.0f;
    public int attackDamage = 10;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    public int maxHealth = 100;
    public AudioSource damageAudioSource;

    private int currentHealth;
    private int previousHealth;
    private bool isGrounded;
    private bool isDead;
    private Animator animator;
    private ScoreManager scoreManager;
    private Transform currentTarget;
    private float lastAttackTime;
    private NavMeshAgent navMeshAgent;
    private Transform spawnPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        scoreManager = GameObject.FindWithTag(TagConstants.WORLD_MANAGER)?.GetComponent<ScoreManager>();
        currentHealth = maxHealth;
        previousHealth = currentHealth;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    public override void FixedUpdateNetwork()
    {
        GameObject WorldManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER);
        spawnPoint = WorldManager.GetComponent<EnemyPrefabSpawner>().spawnTarget.transform;
        float radius = WorldManager.GetComponent<WorldManager>().planeRadius;
        float distance = GetDistance(WorldManager.GetComponent<WorldManager>().mainPlane, transform.position);
        float nextDistance = GetDistance(WorldManager.GetComponent<WorldManager>().mainPlane.GetComponent<PlaneInformation>().nextPlane, transform.position);
        if (distance > radius && distance > nextDistance)
        {
            transform.position = spawnPoint.position;
        }
        if (isDead)
        {
            transform.position = spawnPoint.position;
            currentHealth = maxHealth;
            animator.SetTrigger("respawn");
            isDead = false;
        }

        if (currentHealth != previousHealth)
        {
            OnHealthChanged();
            previousHealth = currentHealth;
        }

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);
        UpdateCurrentTarget();
        HandleMovement();

        if (isGrounded && IsObstacleAhead())
        {
            Jump();
        }
    }

    private float GetDistance(GameObject plane, Vector3 trainPosition)
    {
        try
        {
            Vector3 direction = plane.transform.position - trainPosition;
            return direction.magnitude;
        }
        catch
        {
            return -1;
        }
    }

    private void UpdateCurrentTarget()
    {
        currentTarget = FindClosestTarget();
    }

    private Transform FindClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (string tag in targetTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = potentialTarget.transform;
                }
            }
        }

        return closestTarget;
    }

    private void HandleMovement()
    {
        if (currentTarget == null) return;

        navMeshAgent.SetDestination(currentTarget.position);

        if (navMeshAgent.remainingDistance > 0.5f)
        {
            animator.SetBool(AnimationConstants.IS_RUNNING, true);
        }
        else
        {
            animator.SetBool(AnimationConstants.IS_RUNNING, false);
            TryAttackTarget();
        }
    }

    private void MoveTowardsTarget(Vector3 direction)
    {
        direction.Normalize();
        transform.position += direction * moveSpeed * Runner.DeltaTime;
        animator.SetBool(AnimationConstants.IS_RUNNING, true);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Runner.DeltaTime * moveSpeed);
    }

    private void TryAttackTarget()
    {
        if (Time.time - lastAttackTime > 1.0f)
        {
            lastAttackTime = Time.time;
            if (Runner.IsServer)
            {
                RPC_TakeDamage(currentTarget.GetComponent<NetworkObject>().Id, attackDamage);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_TakeDamage(NetworkId targetId, int damageAmount)
    {
        var targetHealth = Runner.FindObject(targetId).GetComponent<PlayerHealth>();
        targetHealth?.TakeDamage(damageAmount);
    }

    private void Jump()
    {
        // Implement jump logic (e.g., apply force)
    }

    private bool IsObstacleAhead()
    {
        return Physics.Raycast(transform.position, transform.forward, 1.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag(TagConstants.PLAYER_DAMAGE_COLLIDER))
        {
            PerformAttack();
        }
        else if (other.CompareTag(TagConstants.WEAPON))
        {
            TakeDamage(10);
        }
    }

    private void PerformAttack()
    {
        animator.SetTrigger(AnimationConstants.ATTACK);
        TrainHealth trainHealth = GameObject.FindGameObjectWithTag(TagConstants.PLAYER_DAMAGE_COLLIDER).GetComponent<TrainHealth>();
        trainHealth.TakeDamage();
        // Despawn();
        isDead = true;
    }


    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        if (damageAudioSource != null)
        {
            damageAudioSource.Play();
        }

        currentHealth -= damageAmount;
        OnHealthChanged();
    }

    private void OnHealthChanged()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(AnimationConstants.GET_HIT);
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger(AnimationConstants.DIE);
        scoreManager?.AddBonusPoints(10);
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // Despawn();
    }

    private void Despawn()
    {
        EnemyPrefabSpawner spawner = GameObject.FindWithTag(TagConstants.WORLD_MANAGER)?.GetComponent<EnemyPrefabSpawner>();
        spawner.Despawn(this.gameObject);
    }
}