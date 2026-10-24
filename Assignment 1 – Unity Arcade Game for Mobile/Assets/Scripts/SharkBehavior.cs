using UnityEngine;

// A shark in the formation. It looks straight down, fires a bubble when it can see the
// player, and is destroyed when a torpedo hits it.
public class SharkBehavior : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float minFireDelay = 1.5f;
    [SerializeField] private float maxFireDelay = 4.0f;
    [SerializeField] private float sightDistance = 20.0f;

    [Header("Loot")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject pearlPrefab;
    [SerializeField, Range(0.0f, 1.0f)] private float coinChance = 0.3f;
    [SerializeField, Range(0.0f, 1.0f)] private float pearlChance = 0.1f;

    [SerializeField] private int killPoints = 10;
    [SerializeField] private GameObject explosionPrefab;

    private SharkFormation formation;
    private LayerMask sightMask;
    private float halfHeight;
    private float fireTimer;
    void Start()
    {
        formation = GetComponentInParent<SharkFormation>();
        sightMask = LayerMask.GetMask("Player", "Enemy");
        halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        // Random first reload so the sharks don't all fire together.
        fireTimer = Random.Range(minFireDelay, maxFireDelay);
    }

    void Update()
    {
        // No shooting while the formation is still swimming in.
        if (formation != null && formation.IsEntering)
        {
            return;
        }

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0.0f)
        {
            TryShoot();
        }
    }

    // Shooting 
    private void TryShoot()
    {
        // Starts just below the mouth so the ray doesn't hit the shark's own collider.
        Vector2 eye = (Vector2)transform.position + Vector2.down * (halfHeight + 0.1f);

        if (!CanSeePlayer(eye))
        {
            return;
        }

        // The formation only lets one shark fire at a time.
        if (formation == null || formation.RequestShot())
        {
            Instantiate(bubblePrefab, eye, Quaternion.identity);
            fireTimer = Random.Range(minFireDelay, maxFireDelay);
        }
    }

     // Only fires when it see's player.
    // A shark underneath blocks the view, so only the bottom shark in a column shoots.
    private bool CanSeePlayer(Vector2 eye)
    {
        RaycastHit2D hit = Physics2D.Raycast(eye, Vector2.down, sightDistance, sightMask);
        Debug.DrawRay(eye, Vector2.down * sightDistance, Color.red);

        return hit.collider != null && hit.collider.GetComponentInParent<PlayerBehavior>() != null;
    }

    // Getting hit 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TorpedoBehavior>() != null)
        {
            GameController.AddPoints(killPoints);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            DropLoot();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void DropLoot()
    {
        float roll = Random.value;

        if (roll < pearlChance)
        {
            Instantiate(pearlPrefab, transform.position, Quaternion.identity);
        }
        else if (roll < pearlChance + coinChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }

}