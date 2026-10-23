using UnityEngine;

// The player's torpedo: flies straight up and removes itself once it leaves the screen.
// Hitting sharks is handled in SharkBehavior.
public class TorpedoBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 8.0f;
    private Rigidbody2D rb;
    private Camera cam;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        rb.linearVelocity = Vector2.up * speed;
    }

    void Update()
    {
        float top = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y;

        // Wait's until torpedo is off screen then destorys it.
        if (transform.position.y > top + 1.0f)
        {
            Destroy(gameObject);
        }
    }
}