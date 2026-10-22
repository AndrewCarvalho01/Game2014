using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalForce = 20.0f;
    [Range(0.0f, 1.0f)] public float decay = 0.1f;

    private Rigidbody2D rb;
    private Camera cam;
    private float halfWidth;
    private float halfHeight;

    [Header("Weapon")]
    [SerializeField] private GameObject torpedoPrefab;
    [SerializeField] private float fireCooldown = 0.3f;

    private float fireTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        // Size of the sub's sprite
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;
    }

    void Update()
    {
        fireTimer -= Time.deltaTime;

        bool isMoving = GetTouchDirection() != 0.0f;

        if (isMoving && fireTimer <= 0.0f)
        {
            Fire();
            fireTimer = fireCooldown;
        }
    }

    private void Fire()
    {
        Vector3 nose = transform.position + Vector3.up * halfHeight;
        Instantiate(torpedoPrefab, nose, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        // Movement uses physics, so it stays in FixedUpdate.
        rb.linearVelocity *= (1.0f - decay);
        Move();
        CheckBounds();
    }

    // -1 when holding the left half of the screen, +1 for the right half, 0 when not touching.
    private float GetTouchDirection()
    {
        if (Touchscreen.current == null)
        {
            return 0.0f;
        }

        var touch = Touchscreen.current.primaryTouch;

        if (!touch.press.isPressed)
        {
            return 0.0f;
        }

        Vector2 pos = touch.position.ReadValue();

        if (pos.x < Screen.width / 2)
        {
            return -1.0f;
        }

        return 1.0f;
    }

    private void Move()
    {
        float x = GetTouchDirection();
        rb.AddForce(new Vector2(x * horizontalForce, 0.0f));
    }

    private void CheckBounds()
    {
        // Screen edges from the camera, so this works in portrait and landscape.
        float min = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x + halfWidth;
        float max = cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x - halfWidth;

        Vector2 pos = rb.position;

        if (pos.x < min || pos.x > max)
        {
            pos.x = Mathf.Clamp(pos.x, min, max);
            rb.position = pos;
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
    }
}