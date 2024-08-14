using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.ReloadAttribute;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2.0f;
    public int damage = 10;
    public float jumpForce = 5.0f;
    public LayerMask groundLayer;
    public int health = 100;


    //private Rigidbody rb;
    private bool isGrounded;
    private Animator animator;
    private bool isDead = false;
    private ScoreManager scoreManager;
    private EnemyFactory enemyFactory;

    private float lastAttackTime = 0.0f;

    void Start()
    {
        // rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindWithTag(TagConstants.Player2Name).transform;
        }
        scoreManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ScoreManager>();
        enemyFactory = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<EnemyFactory>();
    }

    void Update()
    {
        if (isDead) return;

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.magnitude > 0.5f)
        {
            // move in direction of the player
            direction.Normalize();
            transform.position += direction * speed * Time.deltaTime;
            animator.SetBool(AnimationConstants.IS_RUNNING, true);

            // turn model in the direction of player
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
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
            PlayerHealth playerHealth = GameObject.FindWithTag(TagConstants.Player2Name).GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
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
                TakeDamage(weapon.getDamage());
            }
        }

    }

    void Jump()
    {
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsObstacleAhead()
    {
        return Physics.Raycast(transform.position, transform.forward, 1.0f);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        health -= amount;
        animator.SetTrigger(AnimationConstants.GET_HIT);

        if (health <= 0)
        {
            Die();

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
        gameObject.SetActive(false);
        enemyFactory.EnemyDied(gameObject);
    }
}
