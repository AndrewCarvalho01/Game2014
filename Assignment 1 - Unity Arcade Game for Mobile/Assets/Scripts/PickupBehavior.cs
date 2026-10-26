using UnityEngine;

// A coin or pearl dropped by a shark. It sinks, is collected when the sub touches it,
// and removes itself if it sinks off the bottom of the screen.
public class PickupBehavior : MonoBehaviour
{
    [SerializeField] private float sinkSpeed = 2.0f;
    [SerializeField] private int points = 25;
    [SerializeField] private AudioClip pickupSound;

    private Rigidbody2D rb;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        rb.linearVelocity = Vector2.down * sinkSpeed;
    }

    void Update()
    {
        float bottom = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y;
        if (transform.position.y < bottom - 1.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerBehavior>() != null)
        {
            Destroy(gameObject);
            GameController.AddPoints(points);
            AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        }
    }
}