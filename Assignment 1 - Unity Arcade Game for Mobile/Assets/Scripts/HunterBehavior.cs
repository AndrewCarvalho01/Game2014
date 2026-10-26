using UnityEngine;

public class HunterBehavior : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 4.0f;
    [SerializeField] private float maxSpeed = 4.0f;
    [SerializeField] private float maxSteer = 6.0f;

    private Transform player;
    private SharkFormation formation;
    private Vector2 velocity;
    private bool attacking;

    void Start()
    {
        formation = GetComponentInParent<SharkFormation>();

        PlayerBehavior sub = FindAnyObjectByType<PlayerBehavior>();
        if (sub != null)
        {
            player = sub.transform;
        }
    }
    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (!attacking)
        {
            // Wait for the entrance, then watch the detection radius.
            if (formation != null && formation.IsEntering)
            {
                return;
            }

            if (Vector2.Distance(transform.position, player.position) < detectionRadius)
            {
                StartAttack();
            }
        }
        else
        {
            Seek();
        }
    }

    private void StartAttack()
    {
        attacking = true;
        transform.SetParent(null); // leave the formation
        velocity = Vector2.down * maxSpeed * 0.5f; // begin with a dive
    }

    private void Seek()
    {
        // 1. Straight at the sub, at top speed.
        Vector2 toPlayer = (Vector2)player.position - (Vector2)transform.position;
        Vector2 desired = toPlayer.normalized * maxSpeed;

        // 2. How much does it steer to get to player.
        Vector2 steering = Vector2.ClampMagnitude(desired - velocity, maxSteer * Time.deltaTime);

        // 3. Turn a little, then move.
        velocity = Vector2.ClampMagnitude(velocity + steering, maxSpeed);
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // 4. The sprite face is down by default so make sure it's facing accurately.
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90.0f;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehavior sub = other.GetComponentInParent<PlayerBehavior>();

        if (sub != null)
        {
            sub.TakeHit();
            Destroy(gameObject);
        }
    }

}
