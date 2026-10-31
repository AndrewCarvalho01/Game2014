using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Controls the player's ship: touch movement, firing pooled bullets while moving, and lives.
public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalForce = 20.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float decay = 0.1f;

    [Header("Weapon")]
    [SerializeField] private ObjectPool bulletPool;      // the player bullet pool (filled by the factory)
    [SerializeField] private float fireCooldown = 0.3f;

    [Header("Health")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibleTime = 1.5f;

    public int Lives { get; private set; }

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Camera cam;
    private float halfWidth;
    private float halfHeight;
    private float fireTimer;
    private float invincibleTimer;

    void Start()
    {


        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;

        halfWidth = sr.bounds.extents.x;
        halfHeight = sr.bounds.extents.y;

        Lives = maxLives;
    }

    void Update()
    {
        HandleFiring();
        HandleInvincibility();
    }

    void FixedUpdate()
    {
        rb.linearVelocity *= (1.0f - decay);
        Move();
        CheckBounds();
    }

    // ---------- Input ----------

    // -1 for the left half of the screen, +1 for the right half, 0 when not touching.
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

    // ---------- Movement ----------

    private void Move()
    {
        float x = GetTouchDirection();
        rb.AddForce(new Vector2(x * horizontalForce, 0.0f));
    }

    private void CheckBounds()
    {
        // Screen edges from the camera, moved in by half the ship so it stays fully visible.
        float min = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f)).x + halfWidth;
        float max = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x - halfWidth;

        Vector2 pos = rb.position;

        if (pos.x < min || pos.x > max)
        {
            pos.x = Mathf.Clamp(pos.x, min, max);
            rb.position = pos;
            rb.linearVelocity = new Vector2(0.0f, rb.linearVelocity.y);
        }
    }

    // ---------- Weapon ----------

    private void HandleFiring()
    {
        fireTimer -= Time.deltaTime;

        // Fire automatically while a finger is down.
        bool isMoving = GetTouchDirection() != 0.0f;
        if (isMoving && fireTimer <= 0.0f)
        {
            Fire();
            fireTimer = fireCooldown;
        }
    }

    private void Fire()
    {
        // Take a bullet from the pool instead of creating a new one.
        Vector3 nose = transform.position + Vector3.up * halfHeight;
        bulletPool.Get(nose);
    }

    // ---------- Health ----------

    // Called by enemy bullets and enemies when they hit the player.
    public void TakeHit()
    {
        Debug.Log("TakeHit called. Lives before: " + Lives);
        if (invincibleTimer > 0.0f)
        {
            return;
        }

        Lives--;

        if (Lives <= 0)
        {
            SceneManager.LoadScene("End");
            return;
        }

        invincibleTimer = invincibleTime;
    }

    private void HandleInvincibility()
    {
        if (invincibleTimer <= 0.0f)
        {
            return;
        }

        invincibleTimer -= Time.deltaTime;

        // Blink on and off every 0.1 seconds while invincible.
        sr.enabled = Mathf.Repeat(invincibleTimer, 0.2f) > 0.1f;

        if (invincibleTimer <= 0.0f)
        {
            sr.enabled = true;
        }
    }
}