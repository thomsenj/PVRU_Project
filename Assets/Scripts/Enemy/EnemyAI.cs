using System.Collections;
using UnityEngine;
using Fusion;

public class EnemyAI : NetworkBehaviour
{
    public Transform player;
    [SerializeField] private float speed = 2.0f;
    public int damage = 10;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    public int health = 100;

    private bool isGrounded;
    private Animator animator;
    private bool isDead = false;
    private ScoreManager scoreManager;
    private EnemyFactory enemyFactory;

    private float lastAttackTime = 0.0f;

    [Networked]
    public int NetworkedHealth { get; set; }

    private int previousHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindWithTag(TagConstants.Player2Name).transform;
        }
        scoreManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ScoreManager>();
        enemyFactory = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<EnemyFactory>();

        NetworkedHealth = health;
        previousHealth = NetworkedHealth;
    }

    public override void FixedUpdateNetwork()
    {
        if (isDead) return;

        // Check if health has changed
        if (NetworkedHealth != previousHealth)
        {
            OnHealthChanged();
            previousHealth = NetworkedHealth;
        }

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.magnitude > 0.5f)
        {
            direction.Normalize();
            transform.position += direction * speed * Runner.DeltaTime;
            animator.SetBool(AnimationConstants.IS_RUNNING, true);

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Runner.DeltaTime * speed);
        }
        else
        {
            animator.SetBool("isRunning", false);
            AttemptAttackOnPlayer();
        }
        if (isGrounded && IsObstacleAhead())
        {
            Jump();
        }
    }

    private void AttemptAttackOnPlayer()
    {
        if (Time.time - lastAttackTime > 1.0f)
        {
            lastAttackTime = Time.time;
            if (Runner.IsServer)
            {
                RPC_TakeDamage(player.GetComponent<NetworkObject>().Id, damage);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_TakeDamage(NetworkId playerId, int damageAmount)
    {
        var playerHealth = Runner.FindObject(playerId).GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConstants.PLAYER_DAMAGE_COLLIDER) && !isDead)
        {
            Attack();
        }
        if (other.CompareTag(TagConstants.WEAPON) && !isDead)
        {
            Weapon weapon = GameObject.FindWithTag(TagConstants.Player2Name).GetComponent<Weapon>();
            if (weapon.IsSwinging())
            {
                weapon.setSwinging(false);
                if (Runner.IsServer)
                {
                    RPC_TakeDamageFromWeapon(weapon.getDamage());
                }
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_TakeDamageFromWeapon(int damageAmount)
    {
        TakeDamage(damageAmount);
    }

    void Jump()
    {
        // Handle Jump logic (e.g., apply force)
    }

    bool IsObstacleAhead()
    {
        return Physics.Raycast(transform.position, transform.forward, 1.0f);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        NetworkedHealth -= amount;
    }

    private void OnHealthChanged()
    {
        if (NetworkedHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(AnimationConstants.GET_HIT);
        }
    }

    void Attack()
    {
        animator.SetTrigger(AnimationConstants.ATTACK);
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger(AnimationConstants.DIE);
        scoreManager.AddBonusPoints(10);
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Runner.Despawn(Object);
        enemyFactory.EnemyDied(gameObject);
    }
}
