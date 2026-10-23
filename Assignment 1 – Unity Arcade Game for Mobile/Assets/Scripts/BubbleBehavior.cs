using UnityEngine;
using UnityEngine.SceneManagement;
public class BubbleBehavior : MonoBehaviour
{

    [SerializeField] private float speed = 8.0f;
    private Rigidbody2D rb;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        rb.linearVelocity = Vector2.down * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float bottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
        if (transform.position.y < bottom - 1.0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehavior player = other.GetComponentInParent<PlayerBehavior>();

        if (player != null)
        {
            player.TakeHit();
            Destroy(gameObject);
        }
    }
}
