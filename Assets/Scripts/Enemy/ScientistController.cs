using System.Collections;
using UnityEngine;

public class ScientistController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 origin;
    public float wanderRadius = 5f;
    public float minTimeUntilWander = 3f;
    public float maxTimeUntilWander = 10f;
    public float walkSpeed = 2f;
    public float minWalkDistance = 0.5f;
    public float deathAnimationLength = 0.5f;
    public float playerVelocityAbsorbtion = 0.2f;
    public float timerValue = 3f;
    private bool dead;

    private float idleTimer = 0f;
    private float timeUntilWander = 0f;
    private Vector2 wanderDestination;

    private Animator animator;
    [SerializeField] private ParticleSystem deathParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        origin = transform.position;
        animator.SetBool("Walking", false);

        timeUntilWander = Random.Range(minTimeUntilWander, maxTimeUntilWander);
        wanderDestination = GenerateWanderTarget();
    }

    Vector2 GenerateWanderTarget()
    {
        Vector2 candidate = origin + Random.Range(-wanderRadius, wanderRadius) * Vector2.right;
        Vector2 currPos = transform.position;
        while(Vector2.Distance(candidate, currPos) < minWalkDistance)
        {
            candidate = origin + Random.Range(-wanderRadius, wanderRadius) * Vector2.right;
        }
        return candidate;
    }

    // Update is called once per frame
    void Update()
    {
        if(dead) return;
        if(idleTimer > timeUntilWander)
        {
            // Walk to wander point
            Vector2 currPos = transform.position;
            rb.linearVelocity = walkSpeed * (wanderDestination - currPos).normalized;
            animator.SetBool("Walking", true);

            if(rb.linearVelocity.x > 0.1f) spriteRenderer.flipX = false;
            else if (rb.linearVelocity.x < -0.1f) spriteRenderer.flipX = true;

            if((currPos - wanderDestination).sqrMagnitude < 0.01f)
            {
                animator.SetBool("Walking", false);
                rb.linearVelocity = Vector2.zero;
                idleTimer = 0f;
                timeUntilWander = Random.Range(minTimeUntilWander, maxTimeUntilWander);
                wanderDestination = GenerateWanderTarget();
            }
        }
        else idleTimer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerMovement player = col.GetComponent<PlayerMovement>();
            
            player.TriggerKillEnemy();
            var playerRB = player.GetComponent<Rigidbody2D>();
            Vector2 velocity = playerRB.linearVelocity;
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.linearVelocity = velocity * playerVelocityAbsorbtion;

            // Trigger death
            StartCoroutine(DeathRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        animator.SetTrigger("Death");
        dead = true;
        TimerManager.Instance.AddToTimer(timerValue);

        // Wait until animations complete
        float t = 0f;
        while(t < deathAnimationLength)
        {
            t += Time.deltaTime;
            yield return null;
        }

        deathParticles.Play();
        yield return null;

        // prevent particles from getting destroyed with parent
        deathParticles.transform.parent = null;

        Destroy(gameObject);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
