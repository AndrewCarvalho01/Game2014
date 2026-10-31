using UnityEngine;

// The enemy flies in from above, settles into its row near the top of the screen,
// weaves left and right, and shoots down at random intervals.
// After a while it flies back up and returns to its pool so the spawner can reuse it.
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Boundary speedRange;                                          // side-to-side speed
    [SerializeField] private Boundary fireIntervalRange = new Boundary { min = 1.5f, max = 3.5f }; // seconds between shots
    [SerializeField] private Boundary lifetimeRange = new Boundary { min = 8f, max = 15f };       // seconds before leaving
    [SerializeField] private float flySpeed = 3f;                                          // speed when entering/leaving

    private const float MinFireInterval = 0.5f; // never shoot faster than this

    // Set by the spawner when the enemy is taken from the pool.
    [HideInInspector] public ObjectPool bulletPool;
    [HideInInspector] public float targetY;

    private float speed, timeOffset, halfWidth, halfHeight, fireTimer, lifeTimer;
    private bool leaving;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
    }

    void OnEnable()
    {
        // Runs every time the enemy comes out of the pool, so each reuse starts fresh.
        speed = Random.Range(speedRange.min, speedRange.max);
        timeOffset = Random.Range(0f, 10f);
        fireTimer = NextFireInterval();
        lifeTimer = Random.Range(lifetimeRange.min, lifetimeRange.max);
        leaving = false;
    }

    void Update()
    {
        // Side to side between the screen edges.
        float min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x + halfWidth;
        float max = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - halfWidth;
        float x = min + Mathf.PingPong((Time.time + timeOffset) * speed, max - min);

        float y = transform.position.y;

        if (!leaving)
        {
            // Fly down into the assigned row, then stay there.
            y = Mathf.MoveTowards(y, targetY, flySpeed * Time.deltaTime);

            bool inPosition = Mathf.Approximately(y, targetY);
            if (inPosition)
            {
                Shoot();

                lifeTimer -= Time.deltaTime;
                if (lifeTimer <= 0f)
                {
                    leaving = true;
                }
            }
        }
        else
        {
            // Fly back up off the top of the screen, then go back to the pool.
            y += flySpeed * Time.deltaTime;

            float top = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
            if (y > top + halfHeight)
            {
                GetComponent<PooledObject>().ReturnToPool();
                return;
            }
        }

        transform.position = new Vector2(x, y);
    }

    private void Shoot()
    {
        if (bulletPool == null)
        {
            return;
        }

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            // Fire from just in front of the ship's nose (it points down).
            bulletPool.Get(transform.position + Vector3.down * halfHeight);
            fireTimer = NextFireInterval();
        }
    }

    private float NextFireInterval()
    {
        float interval = Random.Range(fireIntervalRange.min, fireIntervalRange.max);
        return Mathf.Max(interval, MinFireInterval);
    }

    // Enemy rams the player: the player loses a life and the enemy goes back to its pool.
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehavior player = other.GetComponentInParent<PlayerBehavior>();

        if (player != null)
        {
            player.TakeHit();
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
}