using UnityEngine;

// The player's bullet: flies straight up, destroys an enemy it hits (and scores points),
// and goes back to its pool when it hits something or leaves the top of the screen.
public class PlayerBulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private int pointsPerEnemy = 10;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        float top = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f)).y;
        if (transform.position.y > top + 1.0f)
        {
            GetComponent<PooledObject>().ReturnToPool();
        }
    }

    // Bullet hits an enemy: points for the player, enemy and bullet both go back to their pools.
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBehavior enemy = other.GetComponentInParent<EnemyBehavior>();

        if (enemy != null)
        {
            GameController.AddPoints(pointsPerEnemy);
            enemy.GetComponent<PooledObject>().ReturnToPool();
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
}