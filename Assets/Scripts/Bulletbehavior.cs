using UnityEngine;

// The enemy's bullet: moves straight down, costs the player a life if it hits them,
// and goes back to its pool when it hits the player or leaves the bottom of the screen.
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        float bottom = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).y;
        if (transform.position.y < bottom - 1.0f)
        {
            GetComponent<PooledObject>().ReturnToPool();
        }
    }

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