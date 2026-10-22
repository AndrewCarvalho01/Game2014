using UnityEngine;

// Moves the enemy bullet straight down and returns it to its pool
// once it leaves the bottom of the screen.
public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        float bottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
        if (transform.position.y < bottom - 1f)
        {
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
}