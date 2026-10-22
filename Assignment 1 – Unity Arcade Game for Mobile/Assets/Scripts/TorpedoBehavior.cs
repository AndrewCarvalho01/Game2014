using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        float top = cam.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
        if (transform.position.y > top + 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
