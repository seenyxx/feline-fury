using UnityEngine;

public class ProjectileSeeker : MonoBehaviour
{
    [Header("Seeking")]
    public Transform target;
    public float anglePerSecond = 135f;
    public LayerMask enemyLayer;
    public float seekRadius = 6f;
    public PlayerProjectile playerProjectile;

    private Rigidbody2D rb;

    // Find target to seek at the start
    private void Start() {
        rb = playerProjectile.rb;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, seekRadius, enemyLayer);

        if (colliders.Length == 0)
        {
            enabled = false;
            return;
        }

        GameObject closestObj;
        float closestDistance = seekRadius + 1f;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestObj = collider.gameObject;
                closestDistance = distance;
                target = closestObj.transform;
            }
        }
    }

    // Constantly home towards the target and change direction
    private void FixedUpdate() {
        if (!target) {
            enabled = false;
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation  = Quaternion.RotateTowards(transform.rotation, targetRotation, anglePerSecond * Time.fixedDeltaTime);

        rb.velocity = transform.right * playerProjectile.projectileSpeed;
    }
}
