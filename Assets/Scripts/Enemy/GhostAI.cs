using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 2f;
    public float decelerationRange = 5f;
    public float decelerationFactor = 0.5f;

    public Transform player; 
    public GameObject teleportationParticleEffect;
    public float teleportationRange = 5f;
    public int enemyEntityLayer = 12;

    public float teleportCoolDownMin = 5f;
    public float teleportCoolDownMax = 15f;

    private float teleportCooldownSecs = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool flipSprite = false;
    private float lastTeleport;


    // Acquire the player for tracking purposes and also enable the teleport ability
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;

        // Randomises teleportation cooldown
        teleportCooldownSecs = Random.Range(teleportCoolDownMin, teleportCoolDownMax);
    }

    // Constant actions every tick
    void FixedUpdate()
    {
        // Allows the ghost to teleport if the chance criteria is met
        int tpRng = Random.Range(0, 10);

        if (tpRng == 2)
        {
            // Teleport
            TeleportAbility();
        }

        if (!player)
            return;

        // Calculate distance to player
        Vector3 direction = (player.position + (Vector3)Random.insideUnitCircle - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Changes speed based on distance from player
        if (distanceToPlayer > stoppingDistance)
        {
            rb.velocity = moveSpeed * Time.fixedDeltaTime * direction;
        }
        else if (distanceToPlayer > stoppingDistance - decelerationRange)
        {
            float decelerationDistance = stoppingDistance - distanceToPlayer;
            float decelerationSpeed = moveSpeed * (decelerationDistance / decelerationRange) * decelerationFactor;
            rb.velocity = (moveSpeed - decelerationSpeed) * Time.fixedDeltaTime * direction;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // Flips the sprite depending on the direction it is moving
        if (rb.velocity.x < 0f)
        {
            flipSprite = false;
        }
        else if (rb.velocity.x > 0f)
        {
            flipSprite = true;
        }
    }

    // Teleports at a random interval
    private void TeleportAbility(bool forced = false)
    {
        if (Time.time < lastTeleport + teleportCooldownSecs && !forced)
        {
            return;
        }
        
        lastTeleport = Time.time;
        Instantiate(teleportationParticleEffect, transform.position, Quaternion.identity);
        transform.position = transform.position + (Vector3)(Random.insideUnitCircle * teleportationRange);
        Instantiate(teleportationParticleEffect, transform.position, Quaternion.identity);
    }

    // Constantly updates the direction of the sprite
    private void Update() {
        spriteRenderer.flipX = flipSprite;
    }

    // Checks if collides with another enemy and if so then teleports
    private void OnTriggerEnter2D(Collider2D other) {
        // Teleports on collision with other enemy
        if (other.gameObject.layer == enemyEntityLayer)
        {
            TeleportAbility(true);
        }
    }
}
